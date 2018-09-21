/*
 * Author: Karim Dabboussi
 * Date Created: 8/16/2018 @ 12:00 pm
 * Date Modified: 8/16/2018 @ 12:00 pm
 * Project: CompSciClubSpring2018
 * File: TurretMovement.cs
 * Description:  This script makes the enemy strafe within a specified movement positions. The enemy will walk horizontal to the surface you want it to 
 * strafe on, provided that it is placed perpendicular to that surface (otherwise it will not work). Its speed can be changed from within unity if 
 * it needs to strafe faster.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMovement : MonoBehaviour {

    public GameObject point1;
    public GameObject point2;
    private Vector3 tempPosition;
    public float horizontalSpeed = 0.1f;
    public float progress = 0f;
    private Vector3 pos1;
    private Vector3 pos2;
  

    void Start()
    {
        tempPosition = transform.position;
        //default position
        float x = point1.transform.position.x;
        float y = point1.transform.position.y;
        float x2 = point2.transform.position.x;
        float y2 = point2.transform.position.y;
        pos1 = new Vector3(x, y, 0);
        pos2 = new Vector3(x2, y2, 0);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        progress = Mathf.PingPong(Time.time * horizontalSpeed, 1f);
        tempPosition.x = Mathf.Lerp(pos1.x, pos2.x, progress * 1);
        transform.position = Movement(tempPosition.x);
    }
    public Vector3 Movement(float x)
    {
        return new Vector3(x, transform.position.y, 0f);
    }
}
 


