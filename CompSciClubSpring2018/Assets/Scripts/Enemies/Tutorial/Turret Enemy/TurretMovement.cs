/*
 * Author: Keiran Glynn
 * Date Created: 4/21/2018 @ 12:15 pm
 * Date Modified: 4/21/2018 @ 12:30 pm
 * Project: CompSciClubSpring2018
 * File: TurretMovement.cs
 * Description:  This script makes the enemy strafe within a specified movement range. The enemy will walk horizontal to the surface you want it to 
 * strafe on, provided that it is placed perpendicular to that surface (otherwise it will not work). Its speed can be changed from within unity if 
 * it needs to strafe faster.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMovement : MonoBehaviour {
    private Vector2 currTurretPos;
    private Vector2 startTurretPos;
    private float turretRot;
    private Transform turretRB;
    private float minRange = 0f;
    private float maxRange = 0f;
    private int direction = -1;

    public float patrolRange = 5f;
    public float speed = 2f;


	// Use this for initialization
	void Start ()
    {
		turretRB = GameObject.Find("TurretEnemy").GetComponent<Transform>();
        startTurretPos = GameObject.Find("TurretEnemy").GetComponent<Transform>().position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        OnPatrol(patrolRange);
	}

    private void OnPatrol(float range)
    {
        Debug.Log("I should be moving");
        currTurretPos = GameObject.Find("TurretEnemy").GetComponent<Transform>().position;
        turretRot = GameObject.Find("TurretEnemy").GetComponent<Transform>().eulerAngles.z;
        range = patrolRange;
        minRange = -1 * range;
        maxRange = range;
        if ((turretRot == 0) || (turretRot == 180))
        {
            switch (direction)
            {
                case -1:
                    if (currTurretPos.x > (startTurretPos.x + minRange))
                    {
                        turretRB.transform.Translate(-transform.right * Time.deltaTime * speed);
                    }
                    else
                    {
                        direction = 1;
                    }
                    break;
                case 1:
                    if (currTurretPos.x < (startTurretPos.x + maxRange))
                    {
                        turretRB.transform.Translate(transform.right * Time.deltaTime * speed);
                    }
                    else
                    {
                        direction = -1;
                    }
                    break;
            }
        }
        else if ((turretRot == 90) || (turretRot == 270))
        {
            switch (direction)
            {
                case -1:
                    if (currTurretPos.y > (startTurretPos.y + minRange))
                    {
                        turretRB.transform.Translate(transform.up * Time.deltaTime * speed);
                    }
                    else
                    {
                        direction = 1;
                    }
                    break;
                case 1:
                    if (currTurretPos.y < (startTurretPos.y + maxRange))
                    {
                        turretRB.transform.Translate(-transform.up * Time.deltaTime * speed);
                    }
                    else
                    {
                        direction = -1;
                    }
                    break;
            }
        }
        else
        {
            Debug.Log("Fix the rotation of TurretEnemy to be perpendicular to surface");
        }
    }
}
