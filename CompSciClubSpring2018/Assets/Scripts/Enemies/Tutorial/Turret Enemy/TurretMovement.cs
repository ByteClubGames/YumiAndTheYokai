/*
 * Author: Keiran Glynn & Karim Dabboussi
 * Date Created: 4/21/2018 @ 12:15 pm
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

    public Vector3 tempPosition;
    public float horizontalSpeed = 0.1f;
    public float progress = 0f;
    public Vector3 pos1 = new Vector3(-4f, 0f, 0f);
    public Vector3 pos2 = new Vector3(4f, 0f, 0f);

    void Start()
    {
        //default position
        tempPosition = transform.position;
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
 


