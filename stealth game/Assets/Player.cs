using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Player : MonoBehaviour
{
    public event System.Action OnReachedEndOfLevel;
    
    public float moveSpeed = 7;
    public float smoothMoveTime = .1f;
    public float turnSpeed = 8;

    LensDistortion lensDist;

    float angle;
    float smoothInputMagnitude;
    float smoothMoveVelocity;

    public bool hasKey;
    bool hasSpeedBoost;
    bool speedBoostBool;
    bool currentlyBoosting;


    public AnimationCurve curve;
    
    public PostProcessVolume ppv;
    

    Vector3 velocity;

    Rigidbody rigidBody;

    bool disabled;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        Guard.OnGuardHasSpottedPlayer += Disable;
        ppv = GetComponent<PostProcessVolume>();
        lensDist = ppv.profile.GetSetting<LensDistortion>();
        

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputDirection = Vector3.zero;
            if (!disabled) {
            inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }
         
        float inputMagnitude = inputDirection.magnitude;
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);
        
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);

        velocity = transform.forward * moveSpeed * smoothInputMagnitude;

        if (Input.GetKeyDown(KeyCode.Space) && hasSpeedBoost || Input.GetButtonDown("Fire2") && hasSpeedBoost) {
            speedBoostBool = true;
            hasSpeedBoost = false;
            StartCoroutine(LensFlairCoroutine());
            
        }
        //print(ppv.profile.GetSetting<LensDistortion>().intensity.value);
    }
    private void FixedUpdate()
    {
        rigidBody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rigidBody.MovePosition(rigidBody.position + velocity * Time.deltaTime);
        if (speedBoostBool)
        {
            PlayerDash();
            speedBoostBool = false;
        }
    }
     void OnTriggerEnter(Collider hitCollider)
    {
        if(hitCollider.tag == "Finish" && hasKey) 
        {
            Disable();
            if (OnReachedEndOfLevel != null) 
            {
                OnReachedEndOfLevel();

            }
            hasKey = false;
        }
        if(hitCollider.tag == "Key") 
        {
            hasKey = true;
            print("haskey");
            Destroy(GameObject.FindGameObjectWithTag("Key"));
        }
        if (hitCollider.tag == "Speedboost")
        {
            hasSpeedBoost = true;           
           // Destroy(GameObject.FindGameObjectWithTag("Speedboost"));
        }
    }
    void Disable() 
    {
        disabled = true;
    }
    private void OnDestroy()
    {
        Guard.OnGuardHasSpottedPlayer -= Disable;           
    }
    void PlayerDash() 
    {
        rigidBody.AddForce(transform.forward * 15, ForceMode.Impulse);
    }
    IEnumerator LensFlairCoroutine()
    {
        float timer = 0;
        float originalValue = lensDist.intensity.value;
        float duration = 2f;
        float intensity = 60f;

        while(timer < duration)
        {
            timer += Time.deltaTime;

            lensDist.intensity.value = Mathf.Lerp(originalValue, originalValue + intensity, curve.Evaluate (timer / duration));
            yield return null;           
        }

    }
 
}
