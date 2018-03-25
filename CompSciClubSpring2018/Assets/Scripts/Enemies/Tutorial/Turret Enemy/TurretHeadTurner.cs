using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHeadTurner : MonoBehaviour {

    private Quaternion rotation;
    public Transform target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        RotateHead();
	}

    private void RotateHead()
    {
        Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.position, transform.TransformDirection(Vector3.up));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
    }

}
