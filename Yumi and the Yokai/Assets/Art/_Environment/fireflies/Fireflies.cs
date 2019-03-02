using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireflies : MonoBehaviour {

    public GameObject firefly;
	// Use this for initialization
	void Start () {
        Debug.Log(firefly);
        InvokeRepeating("waiter", 0, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
    }

    private void waiter() {
        Instantiate(firefly, this.transform.position, Quaternion.Euler(20, 50, 0));
    }

}
