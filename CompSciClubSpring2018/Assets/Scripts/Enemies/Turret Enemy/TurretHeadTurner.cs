/*
***************************************************************************************
*Creator(s).........................................Keiran Glynn & Karim Dabboussi
*Created..............................................................3/17/2018
*Last Modified............................................@ 12:17 AM on 12/16/2018
*Last Modified by...................................................Karim Dabboussi
*
*Description:  controls the player head rotation.
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
    private Vector2 direction;
    private Vector2 defaultLook;

	// Use this for initialization
	void Start () {
        defaultLook = transform.position;
	}

    void Update ()
    {
        target = this.transform.parent.GetComponent<TurretEnemy>().targeter;
        RotateHead();
	}

    private void RotateHead() //the rotation of the head
    {
        if (target != null)
        {
            direction = target.transform.position - transform.position;
            direction = direction.normalized;
            Quaternion rotation = Quaternion.LookRotation(direction, transform.TransformDirection(Vector3.forward));
            transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        } else { 
            rotation = Quaternion.LookRotation(defaultLook, transform.TransformDirection(new Vector3(0,0,0)));
            transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        }
        
 
    }

}
