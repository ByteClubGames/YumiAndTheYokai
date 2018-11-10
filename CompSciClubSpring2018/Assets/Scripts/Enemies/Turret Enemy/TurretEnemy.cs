/*
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

    // Update is called once per frame
    private void Update()
    {
        projectileSpawn = this.transform.parent.GetChild(0).position;
        rotation = this.transform.parent.GetChild(0).rotation;
        ShootProjectile();
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Human")
        {
            targeter = GameObject.Find("Yumi").transform;
           projectileo.GetComponent<TurretEnemy>().SetInRange(true); //THIS WORKS
            //SetInRange(true);
            Debug.Log("In range of enemy turret");
        }
        else if (collision.tag == "Yokai")
        {
            targeter = GameObject.Find("Yokai(Clone)").transform;
            projectileo.GetComponent<TurretEnemy>().SetInRange(true);
            //SetInRange(true);
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
            Debug.Log("Out of range of enemy turret");
      }
    } // Resets the value of TurretEnemy.inRange to false when the astral goes out of range (leaves box collider)

    private void ShootProjectile()
    {
        if (inRange && Time.time > nextShot) // Provided the target is close enough and a set time has past since the last shot, shoot at target
        {
            nextShot = Time.time + fireRate; // Causes a time delay between each projectile being fired
            Instantiate(projectile, projectileSpawn, rotation);
            Debug.Log("Shots Fired!");
        }
    }

    private void SetInRange(bool incomingValue) // When the yokai is within the detection range of the enemy, TargetYokai.cs will call this function
    {
        inRange = incomingValue;
    }
}
