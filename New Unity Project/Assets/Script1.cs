using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script1 : MonoBehaviour
{
    public int value;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey) 
        {
            value++;
        }
    }
}
