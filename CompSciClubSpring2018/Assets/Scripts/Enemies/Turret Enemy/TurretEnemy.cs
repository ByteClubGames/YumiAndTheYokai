﻿/*
***************************************************************************************
*Creator(s).........................................Keiran Glynn & Karim Dabboussi
*Created..............................................................3/17/2018
*Last Modified............................................@ 4:58PM on 11/9/2018
*Last Modified by...................................................Karim Dabboussi
*
*Description:   This script houses the main control script for the turret enemy.
* It's main purpose is to Istantiate projectile objects when the astral is 
* within range of the turret enemy. 
*               
***************************************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour {

    public GameObject projectile;
    private bool inRange = false;
    private Vector3 projectileSpawn;
    private Quaternion rotation;
    public float fireRate = 1F;
    private float nextShot = 0.0F;
    public Vector3 projPos;
    public Transform targeter;
    public GameObject projectileo;
    public GameObject head;
    public static Vector3 targetDirection;
    public Vector3 projectilePosition;


    // Update is called once per frame
    private void Update()
    {
        //projectileSpawn = this.transform.parent.GetChild(0).position;
        // rotation = transform.root.GetChild(0).rotation;
        //ShootProjectile();
    }
    public Quaternion GetHeadRotation()
    {
        return rotation;
    }
    private void OnTriggerEnter(Collider collision)
    {
        rotation = transform.root.GetChild(1).GetChild(2).rotation;
        projectilePosition = transform.root.GetChild(1).GetChild(2).GetChild(1).position;
        if (collision.tag == "Human")
        {   
            targeter = GameObject.Find("Yumi").transform;
            this.transform.root.GetChild(0).gameObject.GetComponent<TurretProjectile>().IsVisible(true);
            targetDirection = Vector3.down;
        }
        else if (collision.tag == "Ferrox")
        {
            targeter = GameObject.Find("Yokai(Clone)").transform;
            this.transform.root.GetChild(0).gameObject.GetComponent<TurretProjectile>().IsVisible(true);
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Human")
        {
            targeter = GameObject.Find("Yumi").transform;
           projectileo.GetComponent<TurretEnemy>().SetInRange(true); //THIS WORKS
            //SetInRange(true);
            ShootProjectile();
            Debug.Log("In range of enemy turret");
        }
        else if (collision.tag == "Ferrox")
        {
            targeter = GameObject.Find("Yokai(Clone)").transform;
            projectileo.GetComponent<TurretEnemy>().SetInRange(true);
            //SetInRange(true);
            ShootProjectile();
            Debug.Log("In range of enemy turret");
        }
    } 

    private void OnTriggerExit(Collider collision)
      {
          if (collision.tag == "Ferrox" || collision.tag == "Human")
          {
            projectileo.GetComponent<TurretEnemy>().SetInRange(false);
            // SetInRange(false);
            targeter = null;
            this.transform.root.GetChild(0).gameObject.GetComponent<TurretProjectile>().IsVisible(false);
            Debug.Log("Out of range of enemy turret");
      }
    } // Resets the value of TurretEnemy.inRange to false when the astral goes out of range (leaves box collider)

    private void ShootProjectile()
    {
        //if (inRange && Time.time > nextShot) // Provided the target is close enough and a set time has past since the last shot, shoot at target
        // {
        // nextShot = Time.time + fireRate; // Causes a time delay between each projectile being fired
        Debug.Log(this.transform.root.GetChild(0).gameObject);
        this.transform.root.GetChild(0).gameObject.GetComponent<TurretProjectile>().LaunchProjectile(targetDirection);
        //gameObject.transform.Find("Projectile").gameObject.GetComponent<TurretProjectile>().LaunchProjectile();
        Debug.Log("ShootProjectile() has been called");

        //Instantiate(projectile, projectileSpawn, rotation);

        //  }
    }
    private void SetInRange(bool incomingValue) // When the yokai is within the detection range of the enemy, TargetYokai.cs will call this function
    {
        inRange = incomingValue;
    }
}
