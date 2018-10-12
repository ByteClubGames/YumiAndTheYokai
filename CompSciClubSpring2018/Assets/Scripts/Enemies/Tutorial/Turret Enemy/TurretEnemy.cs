/*
 * Author: Keiran Glynn & Karim Dabboussi
 * Date Created: 3/17/2018 @ 11:30 am
 * Date Modified: 3/17/2018 @ 11:30 am
 * Project: CompSciClubSpring2018
 * File: TurretEnemy.cs
 * Description: This script houses the main control script for the turret enemy. It's main purpose is to Istantiate projectile objects when the astral is 
 * within range of the turret enemy.  
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
    public static Transform targeter;

    // Update is called once per frame
    void Update()
    {
        projectileSpawn = this.transform.parent.GetChild(0).position;
        rotation = this.transform.parent.GetChild(0).rotation;
        ShootProjectile();
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Human")
        {
            targeter = GameObject.Find("Player-Human").transform;
            GameObject.Find("ProjectileSpawn").GetComponent<TurretEnemy>().SetInRange(true);
            //SetInRange(true);
            Debug.Log("In range of enemy turret");
        }
        else if (collision.tag == "Yokai")
        {
            targeter = GameObject.Find("Yokai-Human").transform;
            GameObject.Find("ProjectileSpawn").GetComponent<TurretEnemy>().SetInRange(true);
            //SetInRange(true);
            Debug.Log("In range of enemy turret");
        }
    } 

    private void OnTriggerExit(Collider collision)
      {
          if (collision.tag == "Ferrox" || collision.tag == "Human")
          {
            GameObject.Find("ProjectileSpawn").GetComponent<TurretEnemy>().SetInRange(false);
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

    public void SetInRange(bool incomingValue) // When the yokai is within the detection range of the enemy, TargetYokai.cs will call this function
    {
        inRange = incomingValue;
    }
}
