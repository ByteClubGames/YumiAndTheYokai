using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTrigger : MonoBehaviour {
    private bool open = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wind Spell")
        {
            other.GetComponent<DoorOpenAndClose>().openDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Wind Spell")
        {
            other.GetComponent<DoorOpenAndClose>().openDoor();
        }
       
    }

    // Use this for initialization
    

    // Update is called once per frame
    void Update () {
		
	}
}
