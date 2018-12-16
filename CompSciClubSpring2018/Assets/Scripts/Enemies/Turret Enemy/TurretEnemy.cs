/*
***************************************************************************************
*Creator(s).........................................Keiran Glynn & Karim Dabboussi
*Created..............................................................3/17/2018
*Last Modified............................................@ 12:04AM on 12/16/2018
*Last Modified by...................................................Karim Dabboussi
*
*Description:   This script houses the main control script for the turret enemy.
* It's main purpose is to detect an enemy and call a projectile to be shot.        
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
    public Transform targeter;
    public GameObject head;
    public static Vector3 targetDirection;
    public Vector3 projectilePosition;
    public bool playerExit;

    public Quaternion GetHeadRotation()
    {
        return rotation;
    }
    private void OnTriggerEnter(Collider collision)
    {
        rotation = transform.root.GetChild(1).GetChild(2).rotation;//rotation is set to the ProjectileSpawn which rotates with the head
        if (collision.tag == "Human")
        {   
            targeter = GameObject.Find("Yumi").transform; //targeter is given a object to lock onto, should be connected in headturner.cs
            this.transform.root.GetChild(0).gameObject.GetComponent<TurretProjectile>().IsVisible(true);//projectile set to visible
            targetDirection = Vector3.down; //target given
        }
        else if (collision.tag == "Ferrox")
        {
            targeter = GameObject.Find("Yokai(Clone)").transform;
            this.transform.root.GetChild(0).gameObject.GetComponent<TurretProjectile>().IsVisible(true);
            targetDirection = Vector3.down;
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Human") //when the player stays in the range of the trigger projectiles will be shot.
        {
            targeter = GameObject.Find("Yumi").transform;
            ShootProjectile();
            Debug.Log("In range of enemy turret");
        }
        else if (collision.tag == "Ferrox")
        {
            targeter = GameObject.Find("Yokai(Clone)").transform;
            ShootProjectile();
            Debug.Log("In range of enemy turret");
        }
    } 

    private void OnTriggerExit(Collider collision)
      {
          if (collision.tag == "Ferrox" || collision.tag == "Human")
          {
            targeter = null;
            this.transform.root.GetChild(0).gameObject.GetComponent<TurretProjectile>().IsVisible(false);
            playerExit = true;
            if(playerExit == true)// When the player exits the turrents range its projectile is reset to its inital position.
            {
                this.transform.root.GetChild(0).gameObject.GetComponent<TurretProjectile>().OutOfRange(true);
            }
            Debug.Log("Out of range of enemy turret");
      }
    }

    private void ShootProjectile()
    {
        this.transform.root.GetChild(0).gameObject.GetComponent<TurretProjectile>().LaunchProjectile(targetDirection); //Calls launchprojectile to begin the launch phase of the projectile
    }

    private void SetInRange(bool incomingValue) // When the yokai is within the detection range of the enemy, TargetYokai.cs will call this function
    {
        inRange = incomingValue;
    }
}
