//Script causes the object attached to oscillate between waypoints
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    //List of waypoints
    public List<Transform> waypoints;
    //current waypoint
    private int curWaypointIndex = 0;
    //speed
    public float speed = 3f;

    // Update is called once per frame
    void Update()
    {
        //Get the position of the waypoint
        Transform curTarget = waypoints[curWaypointIndex];
        //Move towards it and look at it
        //transform.LookAt(curTarget.position);
        transform.position = Vector3.MoveTowards(transform.position,curTarget.position, speed * Time.deltaTime);
        //If we are close enough to the position
        if(Vector3.Distance(transform.position, curTarget.position) < 0.1f){
            //Move to the next position;
            curWaypointIndex++;
            //If we have hit the end, go back to the beginning.
            if(curWaypointIndex>waypoints.Count-1){
                curWaypointIndex=0;
            }
        }
    }
}
