using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthLayerCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Earth Spell Object" && col.collider == "box")
        {
            // Do something for box collider
        }
        else if (col.gameObject.tag == "Earth Spell Object" && col.collider == "circle")
        {
            // Do something for circle collider
        }
    }
}
