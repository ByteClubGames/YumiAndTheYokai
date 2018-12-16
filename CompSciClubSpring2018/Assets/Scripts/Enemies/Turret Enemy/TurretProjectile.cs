/*
***************************************************************************************
*Creator(s).........................................Keiran Glynn & Karim Dabboussi
*Created..............................................................3/17/2018
*Last Modified............................................@ 4:55PM on 11/9/2018
*Last Modified by...................................................Karim Dabboussi
*
*Description:   This script controls the function of the projectiles (bullets) fired by the turret enemy. 
*It controls how the bullets move and when they should
* be destroyed. It also houses values for how fast the projectiles move towards the player. 
* If a projectile happens to hit the astral, it will 
* cause damage to it. If it hits another object that is not the astral, it will be destroyed.
* 
*
*               
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
    //private Rigidbody humanRB;
    private bool isHit = false; // will indicate if the projectile had a collision
    private bool isTooFar = false; // will indicate if the projectile is far from the astral (if it missed its target)
    private Vector3 projectilePos;
    private Vector3 humanPos;
    private Vector3 target;
    private bool canShoot = false;
    //private GameObject player;

    // Use this for initialization
    private void Start()
    {
        this.transform.position = transform.root.GetChild(1).GetChild(2).GetChild(1).position;
    }
    void Awake ()
    {
        PlayerTransform();
        projectileTransform = GetComponent<Transform>();
        projectilePos = this.GetComponent<Transform>().position;
        //projectilePos = GameObject.GetComponent<TurretEnemy>().projPos + projectileTransform.position;
        
        target = (player.position - projectilePos).normalized; //calculates the target
        Debug.Log(target);
       // target = player.position;
        //projectileTransform.LookAt(-player.position);
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canShoot == true)
        {
            //ProjectileMovement();
        }
    }

    private void Update() // Making sure the projectile didn't hit anything (if it did it gets destroyed)
    {
        //CheckIsHit(); 
        //CheckTooFar();
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



    //private void OnTriggerEnter(Collider collision) // This is supposed to be triggered when the projectile intercepts the astral
    //{
    //    Debug.Log("this works"); // if this trigger function is called at all, this will show up in the console
    //    if (collision.gameObject.name == "Ferrox" || collision.tag == "Human")
    //    {
    //        //ProjectileMovement(); disabled to test
    //        GameObject.Find("Ferrox").GetComponent<FerroxHealth>().TakeDamage(projectileDamage);
    //        GameObject.Find("Yumi").GetComponent<HumanHealth>().TakeDamage(projectileDamage);
    //        isHit = true; // regardless of weather or not it hit the astral, it will be as having hit someting
    //    }
    //    else if (collision.gameObject.tag == "EnemyDetection")
    //    {
    //        // Do nothing
    //    }
    //}

    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Human" || collision.tag == "Ferrox")
        {
            
            GameObject.Find("Ferrox").GetComponent<FerroxHealth>().TakeDamage(projectileDamage);
            GameObject.Find("Yumi").GetComponent<HumanHealth>().TakeDamage(projectileDamage);
            isHit = true;
            if(isHit == true)
            {
                transform.position = transform.root.GetChild(1).GetChild(2).GetChild(1).position;
                transform.rotation = transform.root.GetChild(1).GetChild(2).GetChild(1).rotation;
                this.IsVisible(false);
                canShoot = false;
            }
        }
        else 
        {
            Debug.Log("No Collision");
        }
    }
    public void LaunchProjectile(Vector3 targetDirection)
    {
        //transform.position = transform.root.GetChild(2).GetChild(1).GetChild(1).position;
        transform.rotation = transform.root.GetChild(1).gameObject.GetComponent<TurretEnemy>().GetHeadRotation();
        //this.transform.root.GetChild(0).gameObject.GetComponent<TurretProjectile>().IsVisible(true);
        Debug.Log("Projectile Position:" + transform.position);
        Debug.Log("Projectile Spawn:" + transform.root.GetChild(1).GetChild(2).GetChild(1).position);
        this.IsVisible(true);
        canShoot = true;
       // Vector3 newVec = Vector3.down;
        ProjectileMovement(targetDirection);
        StartCoroutine(WaitToReturn(timeToDestroy)); //time to destroy is time to return, jsut for testing purposes
    }
    public void ProjectileMovement(Vector3 targetDirection) // Makes the projectile move in a straight line towards the player
    {
        //this.transform.position = this.transform.root.Find("ProjectileSpawn").position;
        // this.transform.rotation = this.transform.root.Find("ProjectileSpawn").gameObject.transform.rotation;
            projectileTransform.Translate(targetDirection * speed * Time.deltaTime); //makes projectile move towards player
      
        //StartCoroutine(waitToDestroy(timeToDestroy)); //deletes object after given time
    }

    /// <summary>
    /// Assigns the player variable to either the Yokai transform (if it exists), or the Yumi transform.
    /// </summary>
    private void PlayerTransform()
    {
        if (GameObject.Find("Ferrox") == null)
        {
            player = GameObject.Find("Yumi").GetComponent<Transform>();
        }
        else
        {
            player = GameObject.Find("Ferrox").GetComponent<Transform>();
        }
    }

        /// <this section here is outdated>
        //private void CheckIsHit() // Destroys the projectile if it hits somthing
        //{
        //    if (isHit)
        //    {
        //        Destroy(gameObject);
        //    }
        //}

        //private void CheckTooFar() // Will destroy the projectile if it has missed the yokai and is far away
        //{

        //    distance = Vector3.Distance(projectilePos, humanPos);

        //    if (distance > maxDistance)
        //    {
        //        Destroy(gameObject);
        //    }
    //}
    private IEnumerator WaitToReturn(float waitTime)
    {
        
        yield return new WaitForSeconds(waitTime);
        this.transform.position = transform.root.GetChild(1).GetChild(2).GetChild(1).position;
        this.transform.rotation = transform.root.GetChild(1).GetChild(2).GetChild(1).rotation;
        this.IsVisible(false);
        canShoot = false;
    }
}
