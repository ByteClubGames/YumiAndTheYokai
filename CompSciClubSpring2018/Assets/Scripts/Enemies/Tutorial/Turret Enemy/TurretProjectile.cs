/*
 * Author: Keiran Glynn
 * Date Created: 3/17/2018 @ 11:30 am
 * Date Modified: 3/17/2018 @ 11:30 am
 * Project: CompSciClubFall2017
 * File: TurretProjectile.cs
 * Description:
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour {
    public float speed;
    public int projectileDamage = 1;

    private Rigidbody2D turretProjectileRB;
    private Rigidbody2D yokaiRB;
    //private Vector2 yokaiPos;
    //private Vector2 turretprojectilePos;
    private bool isHit = false;
    
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

    private void Update()
    {
        CheckIsHit();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("this works");
        if (collision.gameObject.CompareTag("Ferrox"))
        {
            GameObject.Find("Ferrox").GetComponent<FerroxHealth>().TakeDamage(projectileDamage);
        }

        isHit = true;
    }

    public void ProjectileMovement()
    {
        //turretprojectilePos = turretProjectileRB.position;
        //yokaiPos = yokaiRB.position;

        if(turretProjectileRB.velocity.x == 0f && turretProjectileRB.velocity.y == 0f)
        {
            turretProjectileRB.AddForce((yokaiRB.position - turretProjectileRB.position) * speed, ForceMode2D.Impulse);
        }
    }

    private void CheckIsHit()
    {
        if (isHit)
        {
            Destroy(gameObject);
        }
    }
}
