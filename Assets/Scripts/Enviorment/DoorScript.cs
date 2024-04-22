using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Animator anim;

    void OnTriggerEnter(Collider other){
        //If we have an animator
        if(anim && other.CompareTag("Player")){
            anim.SetBool("DoorOpen", true);
        }
    }
    void OnTriggerExit(Collider other){
        //If we have an animator
        if(anim && other.CompareTag("Player")){
            anim.SetBool("DoorOpen", false);
        }
    }
}
