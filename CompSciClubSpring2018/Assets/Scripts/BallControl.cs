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
	}
}
