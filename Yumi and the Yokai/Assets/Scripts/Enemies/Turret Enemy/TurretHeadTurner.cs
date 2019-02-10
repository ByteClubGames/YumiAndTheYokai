/*
***************************************************************************************
*Creator(s).........................................Karim Dabboussi
*Created..............................................................3/17/2018
*Last Modified............................................@ 11:46 PM on 2/9/2019
*Last Modified by...................................................Karim Dabboussi
*
*Description:  controls the player head rotation and is the turrent.
*
*               
***************************************************************************************
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHeadTurner : MonoBehaviour {

    private Quaternion rotation;

    public Transform target;
    public Transform firePoint;

    private Vector2 direction;
    private Vector2 defaultLook;

    public float range = 25f;
    public float distance;
    public float fireRate = 1f;
    private float fireCountDown = 0f;

    public GameObject bullePrefab;
    
    public Animator headTurrent;

	// Use this for initialization
	void Start () {
        defaultLook = transform.position;
	}

    void Update()
    {
        headTurrent.SetBool("shoot", false);
        Debug.Log("The value of before:" + fireCountDown);
        target = GameObject.Find("Yumi").transform; // Finds Yumi as the target
        distance = Vector3.Distance(this.transform.position, target.position);
        RotateHead();
        if (fireCountDown <= 0f && range >= distance)
        {
            Shoot();
            fireCountDown = (1f / fireRate);
        }

        fireCountDown -= Time.deltaTime;
        Debug.Log("The value of after:" + fireCountDown);
    }
     
    void Shoot()
    {
        GameObject projectileGameObject = (GameObject)Instantiate(bullePrefab, firePoint.position, firePoint.rotation);
        ProjectileScript projectileo = projectileGameObject.GetComponent<ProjectileScript>();
      
        if (projectileo != null)
        {
            headTurrent.SetBool("shoot", true);
            projectileo.Seek(target);
        }
        Debug.Log("Shoot");
    }
    private void RotateHead() //the rotation of the head
    {
        distance = Vector3.Distance(this.transform.position, target.position);
        if (target != null)
        {
            direction = target.transform.position - transform.position;
            direction = direction.normalized;
            Quaternion rotation = Quaternion.LookRotation(direction, transform.TransformDirection(Vector3.forward));
            transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
            if (distance > 25f) //need to set to an actual variable later
            {
                rotation = Quaternion.LookRotation(defaultLook, transform.TransformDirection(new Vector3(0, 0, 0)));
                transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
            }
        }
        
 
    }

}
