/*
*
*Creator(s).........................................................Brenden Plong
*Created..............................................................10/05/2018
*Last Modified............................................@ 12:06AM on 11/2/2018
*Last Modified by...................................................Brenden Plong
*
*Description:   Script that is used to check the direction by shooting out an ray
**
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour {
    private Rigidbody rb;
    public float speed;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float MoveHorizontial = Input.GetAxis("Horizontial");
        float MoveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(MoveHorizontial, 0, MoveVertical);
        rb.AddForce(movement * speed);
        RaycastHit hit;
        //Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        if (Physics.Raycast(rb.transform.position, -Vector3.up, out hit, 100.0f))
            print("Found an object - distance: " + hit.distance);
            Debug.DrawRay(rb.transform.position, movement, Color.green);
	}
}
