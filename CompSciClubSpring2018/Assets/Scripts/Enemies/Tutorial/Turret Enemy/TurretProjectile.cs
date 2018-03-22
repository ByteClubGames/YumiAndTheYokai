/*
 * Author: Keiran Glynn
 * Date Created: 3/17/2018 @ 11:30 am
 * Date Modified: 3/17/2018 @ 11:30 am
 * Project: CompSciClubFall2017
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

    private Rigidbody2D turretProjectileRB;
    private Rigidbody2D yokaiRB;
    private bool isHit = false; // will indicate if the projectile had a collision
    
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
    }

    private void OnCollisionEnter2D(Collision2D collision) // This is supposed to be triggered when the projectile intercepts the astral
    {
        Debug.Log("this works"); // if this trigger function is called at all, this will show up in the console
        if (collision.gameObject.CompareTag("Ferrox"))
        {
            GameObject.Find("Ferrox").GetComponent<FerroxHealth>().TakeDamage(projectileDamage);
        }

        isHit = true; // regardless of weather or not it hit the astral, it will be as having hit someting
    }

    public void ProjectileMovement() // Makes the projectile move in a straight line towards the player
    {        
        if(turretProjectileRB.velocity.x == 0f && turretProjectileRB.velocity.y == 0f) // As long as it isnt already moving: do action
        {
            turretProjectileRB.AddForce((yokaiRB.position - turretProjectileRB.position) * speed, ForceMode2D.Impulse); // Adds an instantaneous force towards yokai
        }
    }

    private void CheckIsHit() // Destroys the projectile if it hits somthing
    {
        if (isHit)
        {
            Destroy(gameObject);
        }
    }
}
