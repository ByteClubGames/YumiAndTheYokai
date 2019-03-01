using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigExplosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void DestroyExplosion() {
        Destroy(this.gameObject);
    }
}
