using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_LookAt : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null){
            transform.LookAt(target);
        }
    }
}
