using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTrigger : MonoBehaviour {
    public double maxopentime;
    private double opentime;
    private bool open = false;

    private void OnTriggerEnter(Collider other)
    {
        if (open)
        {
            open = false;
            return;
        }
        if(other.tag == "WindSpell")
        {
            opentime = Time.fixedTime;
            open = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "WindSpell")
        {
            //open = false;
        }

    }

    // Use this for initialization
    

    // Update is called once per frame
    void Update () {
        if (Time.fixedTime - opentime > maxopentime)
        {
            open = false;
        }
        if (open)
        {
            GameObject.Find("Door").GetComponent<DoorOpenAndClose>().openDoor();
        } else
        {
            GameObject.Find("Door").GetComponent<DoorOpenAndClose>().closeDoor();
        }

    }
}
