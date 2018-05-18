using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTrigger : MonoBehaviour {
    private bool open = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "WindSpell")
        {
            GameObject.Find("Door").GetComponent<DoorOpenAndClose>().openDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "WindSpell")
        {
            GameObject.Find("Door").GetComponent<DoorOpenAndClose>().closeDoor();
        }

    }

    // Use this for initialization
    

    // Update is called once per frame
    void Update () {
		
	}
}
