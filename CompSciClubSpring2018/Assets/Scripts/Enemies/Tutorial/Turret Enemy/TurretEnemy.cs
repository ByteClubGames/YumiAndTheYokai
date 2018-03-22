/*
 * Author: Keiran Glynn
 * Date Created: 3/17/2018 @ 11:30 am
 * Date Modified: 3/17/2018 @ 11:30 am
 * Project: CompSciClubFall2017
 * File: TurretEnemy.cs
 * Description:
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour {

    private Rigidbody2D turretEnemyRB;
    public GameObject projectile;
    private bool inRange = false;
    private Transform projectileSpawn;
    public float fireRate = 0.5F;
    private float nextShot = 0.0F;

	// Use this for initialization
	void Start ()
    {
        turretEnemyRB = GetComponent<Rigidbody2D>();
        projectileSpawn = GameObject.Find("ProjectileSpawn").GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        ShootProjectile();
	}

    private void ShootProjectile()
    {
        if (inRange && Time.time > nextShot)
        {
            nextShot = Time.time + fireRate;
            Instantiate(projectile, projectileSpawn);
            Debug.Log("Shots Fired!");
        }
    }

    public void SetInRange(bool incomingValue)
    {
        inRange = incomingValue;
    }
}
