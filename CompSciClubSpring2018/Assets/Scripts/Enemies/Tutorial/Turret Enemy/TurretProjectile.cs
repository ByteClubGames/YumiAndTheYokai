/*
 * Author: Keiran Glynn
 * Date Created: 3/17/2018 @ 11:30 am
 * Date Modified: 3/17/2018 @ 11:30 am
 * Project: CompSciClubSpring2018
 * File: TurretProjectile.cs
 * Description: This script controls the function of the projectiles (bullets) fired by the turret enemy. It controls how the bullets move and when they should
 * be destroyed. It also houses values for how fast the projectiles move towards the player. If a projectile happens to hit the astral, it will 
 * cause damage to it. If it hits another object that is not the astral, it will be destroyed.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour {
    public float speed; // how fast the projectile flies through the air
    public int projectileDamage = 1;

    public float maxDistance;
    private float distance;
    private Rigidbody2D turretProjectileRB;
    private Rigidbody2D yokaiRB;
    private bool isHit = false; // will indicate if the projectile had a collision
    private bool isTooFar = false; // will indicate if the projectile is far from the astral (if it missed its target)
    private Vector2 projectilePos;
    private Vector2 yokaiPos;
    private Vector2 target;
    
	// Use this for initialization
	void Start ()
    {
        turretProjectileRB = this.GetComponent<Rigidbody2D>();
        yokaiRB = GameObject.Find("Ferrox").GetComponent<Rigidbody2D>();

        turretProjectileRB.gravityScale = 0; 
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        ProjectileMovement();
    }

    private void Update() // Making sure the projectile didn't hit anything (if it did it gets destroyed)
    {
        CheckIsHit();
        CheckTooFar();
    }

    private void OnTriggerEnter2D(Collider2D collision) // This is supposed to be triggered when the projectile intercepts the astral
    {
        Debug.Log("this works"); // if this trigger function is called at all, this will show up in the console
        if (collision.gameObject.name == "Ferrox")
        {
            GameObject.Find("Ferrox").GetComponent<FerroxHealth>().TakeDamage(projectileDamage);
            isHit = true; // regardless of weather or not it hit the astral, it will be as having hit someting
        }
        else if(collision.gameObject.tag == "EnemyDetection")
        {
            // Do nothing
        }        
    }

    public void ProjectileMovement() // Makes the projectile move in a straight line towards the player
    {
        

        if (turretProjectileRB.velocity.x == 0f && turretProjectileRB.velocity.y == 0f) // As long as it isnt already moving: do action
        {
            target = (yokaiRB.position - turretProjectileRB.position);
            target = target.normalized;
            turretProjectileRB.AddForce(target * speed, ForceMode2D.Impulse); // Adds an instantaneous force towards yokai
        }
    }

    private void CheckIsHit() // Destroys the projectile if it hits somthing
    {
        if (isHit)
        {
            Destroy(gameObject);
        }
    }

    private void CheckTooFar() // Will destroy the projectile if it has missed the yokai and is far away
    {
        projectilePos = turretProjectileRB.position;
        yokaiPos = yokaiRB.position;

        distance = Vector2.Distance(projectilePos, yokaiPos);

        if(distance > maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
