using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rayCastForward : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var sphereCol = this.GetComponent<SphereCollider>();
        float radiusDis = sphereCol.bounds.max.x;
        var rayOrigin = transform.position + transform.right * radiusDis;
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        float theDistance;
        Vector3 forward = transform.TransformDirection(Vector3.right) * 2; 
        Debug.DrawRay(transform.position, forward, Color.green);

        if (Physics.Raycast(transform.position, forward, out hit)){
            theDistance = hit.distance;
            print(theDistance + " " + hit.collider.gameObject.name);
        }
	}
}
