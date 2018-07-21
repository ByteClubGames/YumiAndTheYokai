/*
 * Author: Keiran Glynn
 * Date Created: 3/17/2018 @ 11:30 am
 * Date Modified: 3/17/2018 @ 11:30 am
 * Project: CompSciClubSpring2018
 * File: TurretHeadTurner.cs
 * Description: This script is responsible for making the head of the enemy face towards the player at all times.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHeadTurner : MonoBehaviour {

    private Quaternion rotation;
    public Transform target;
    private Vector2 direction;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        RotateHead();
	}

    private void RotateHead()
    {
        direction = target.transform.position - transform.position;
        direction = direction.normalized;
        Quaternion rotation = Quaternion.LookRotation(direction, transform.TransformDirection(Vector3.forward));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
    }

}
