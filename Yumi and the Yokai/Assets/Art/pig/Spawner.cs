using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject obj;
    public float spwan_rate = 1;

	// Use this for initialization
	void Start () {
        InvokeRepeating("spawn", 0, spwan_rate);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void spawn() {
        Instantiate(obj, this.transform.position, Quaternion.identity);
    }
}
