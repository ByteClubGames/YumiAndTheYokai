/*
***************************************************************************************
*Creator(s).........................................Keiran Glynn & Karim Dabboussi
*Created..............................................................3/17/2018
*Last Modified............................................@ 11:32PM on 12/15/2018
*Last Modified by...................................................Karim Dabboussi
*
*Description:   This script controls the function of the projectiles (bullets) fired by the turret enemy. 
*It controls how the projectiles move and appear
***************************************************************************************
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
    private Vector3 projectilePos;
    private Vector3 humanPos;
    private Vector3 target;

    private void Start() //Where the inital projectile is set to on start, not recommended to edit
    {
        this.transform.position = transform.root.GetChild(1).GetChild(2).GetChild(1).position;
    }
    void Awake ()
    {
        PlayerTransform();
        projectileTransform = GetComponent<Transform>();
        projectilePos = this.GetComponent<Transform>().position;
    }

    public void IsVisible(bool option)
    {
        if (option == true)   // this function is to set the turrent projectile to visible and invisible and is utilized in other parts of this program
        {
            this.gameObject.SetActive(true);
        } else {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Human" || collision.tag == "Ferrox")
        {
            Debug.Log("this should happen");
            //GameObject.Find("Yokai(Clone)").GetComponent<FerroxHealth>().TakeDamage(projectileDamage);
           // GameObject.Find("Yumi").GetComponent<HumanHealth>().TakeDamage(projectileDamage); damage does not work, code outdated
                transform.position = transform.root.GetChild(1).GetChild(2).GetChild(1).position; //referencing ProjectileSpawn
                transform.rotation = transform.root.GetChild(1).GetChild(2).GetChild(1).rotation; //refernecing ProjectileSpawn(good for multiple enemies instead of find method)
                this.IsVisible(false);
        }
    }
    public void LaunchProjectile(Vector3 targetDirection) //script called from TurrentEnemy that causes projectile to be launched
    {
        transform.rotation = transform.root.GetChild(1).gameObject.GetComponent<TurretEnemy>().GetHeadRotation();
        this.IsVisible(true);
        ProjectileMovement(targetDirection);
        StartCoroutine(WaitToReturn(timeToDestroy)); //basically time until object is returned to shoot
    }
    public void ProjectileMovement(Vector3 targetDirection) // Makes the projectile move in a straight line towards the player
    {
        projectileTransform.Translate(targetDirection * speed * Time.deltaTime); //makes projectile move towards player
    }
    public void OutOfRange(bool booleanValue) //if the boolean is true then the object returnss to its inital position
    { 
        if(booleanValue == true)
        {
            this.transform.position = transform.root.GetChild(1).GetChild(2).GetChild(1).position;
        }
    }

    private void PlayerTransform()
    {
        if (GameObject.Find("Ferrox") == null) //find the transform of the player or ferrox
        {
            player = GameObject.Find("Yumi").GetComponent<Transform>();
        }
        else
        {
            player = GameObject.Find("Ferrox").GetComponent<Transform>();
        }
    }

    private IEnumerator WaitToReturn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // after a set amount of time the object will return back to inital firing position & rotation
        this.transform.position = transform.root.GetChild(1).GetChild(2).GetChild(1).position;
        this.transform.rotation = transform.root.GetChild(1).GetChild(2).GetChild(1).rotation;
        this.IsVisible(false); // object is set to invisible
    }
}
