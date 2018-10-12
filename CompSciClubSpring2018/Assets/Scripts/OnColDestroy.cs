using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnColDestroy : MonoBehaviour {
    public GameObject player;
    public GameObject enemy;
	// Use this for initialization

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "ColSphere")
        {
            Destroy(enemy);
        }
    }
    // Update is called once per frame
}
