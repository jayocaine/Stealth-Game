using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateConstantly : MonoBehaviour
{
    public Vector3 axis;
    public float speed;

    private Quaternion defaultRotation;
    private void Awake() {
        defaultRotation = transform.localRotation;
    }
    private void Update() {
        float angle = speed * Time.time;
        transform.localRotation = defaultRotation * Quaternion.AngleAxis(angle, axis);
    }
    
}
