using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script2 : MonoBehaviour
{
    public Script1 otherScript;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(otherScript == null)
        {
            Debug.Log("danger danger");
            return;
        }
       //do the stuff from the other script.
       
    }
}
