/*
 * Author: Keiran Glynn
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

    //private Rigidbody2D turretEnemyRB;
    public GameObject projectile;
    private bool inRange = false;
    private Vector2 projectileSpawn;
    private Quaternion rotation;
    public float fireRate = 1F;
    private float nextShot = 0.0F;



	// Use this for initialization
	void Start ()
    {
        //turretEnemyRB = GetComponent<Rigidbody2D>();
        

    }
	
	// Update is called once per frame
	void Update ()
    {
        projectileSpawn = GameObject.Find("ProjectileSpawn").GetComponent<Transform>().position;
        rotation = GameObject.Find("ProjectileSpawn").GetComponent<Transform>().rotation;
        ShootProjectile();
	}

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
