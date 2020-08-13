using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event System.Action OnReachedEndOfLevel;
    private Animator anim;
    
    public float moveSpeed = 7;
    public float smoothMoveTime = .1f;
    public float turnSpeed = 8;

    float angle;
    float smoothInputMagnitude;
    float smoothMoveVelocity;

    bool hasKey;

    Vector3 velocity;

    Rigidbody rigidBody;

    bool disabled;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        Guard.OnGuardHasSpottedPlayer += Disable;
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
        if(Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Vertical") > 0)
        {
            anim.SetBool("isWalking", true);
        }
        
    }
    private void FixedUpdate()
    {
        rigidBody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rigidBody.MovePosition(rigidBody.position + velocity * Time.deltaTime);
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
        }
        if(hitCollider.tag == "Key") 
        {
            hasKey = true;
            print("haskey");
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

}
