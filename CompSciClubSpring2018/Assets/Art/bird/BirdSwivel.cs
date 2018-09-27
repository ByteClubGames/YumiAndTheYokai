using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSwivel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(Vector3.left, Mathf.Sin(Time.time));
    }
}
