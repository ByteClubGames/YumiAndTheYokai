/*
 * Author: Keiran Glynn & Karim Dabboussi
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
    public float timeToDestroy;
    private Transform projectileTransform;
    private Transform player;
    //private Rigidbody humanRB;
    private bool isHit = false; // will indicate if the projectile had a collision
    private bool isTooFar = false; // will indicate if the projectile is far from the astral (if it missed its target)
    private Vector3 projectilePos;
    private Vector3 humanPos;
    private Vector3 target;
    //private GameObject player;

    // Use this for initialization
    void Awake ()
    {
        PlayerTransform();
        projectileTransform = GetComponent<Transform>();
        projectilePos = GameObject.Find("ProjectileSpawn").GetComponent<Transform>().position;// + projectileTransform.position;
        //projectilePos = GameObject.GetComponent<TurretEnemy>().projPos + projectileTransform.position;
        target = (player.position - projectilePos).normalized;
        //projectileTransform.LookAt(-player.position);
        

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

    private void OnTriggerEnter(Collider collision) // This is supposed to be triggered when the projectile intercepts the astral
    {
        Debug.Log("this works"); // if this trigger function is called at all, this will show up in the console
        if (collision.gameObject.name == "Ferrox" || collision.tag == "Human")
        {
            //ProjectileMovement();
            GameObject.Find("Ferrox").GetComponent<FerroxHealth>().TakeDamage(projectileDamage);
            GameObject.Find("Player-Human").GetComponent<HumanHealth>().TakeDamage(projectileDamage);
            isHit = true; // regardless of weather or not it hit the astral, it will be as having hit someting
        }
        else if (collision.gameObject.tag == "EnemyDetection")
        {
            // Do nothing
        }
    }

    public void ProjectileMovement() // Makes the projectile move in a straight line towards the player
    {
        projectileTransform.Translate(target * speed * Time.deltaTime);
        StartCoroutine(waitToDestroy(timeToDestroy));
    }

    /// <summary>
    /// Assigns the player variable to either the Yokai transform (if it exists), or the Yumi transform.
    /// </summary>
    private void PlayerTransform()
    {
        if (GameObject.Find("Ferrox") == null)
        {
            player = GameObject.Find("Player-Human").GetComponent<Transform>();
        }
        else
        {
            player = GameObject.Find("Ferrox").GetComponent<Transform>();
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

        distance = Vector3.Distance(projectilePos, humanPos);

        if (distance > maxDistance)
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator waitToDestroy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
    }
}
