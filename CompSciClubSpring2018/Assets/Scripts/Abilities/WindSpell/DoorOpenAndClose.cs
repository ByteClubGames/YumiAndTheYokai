using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenAndClose : MonoBehaviour {
    public Transform door;
    public Vector3 closedPositon = new Vector3(3.441f, 1.5f, .32f);
    public Vector3 openPositon = new Vector3(3.441f, 3.55f, .32f);
    private float openspeed = 5;
    private bool open = false;
    // Use this for initialization
    void Start () {
        door = GetComponent<Transform>();

    }
	
	// Update is called once per frame
	void Update () {
        if (open)
        {
            door.position = Vector3.Lerp(door.position, openPositon, Time.deltaTime * openspeed);
        } else
        {
            door.position = Vector3.Lerp(door.position, closedPositon, Time.deltaTime * openspeed);
        }
    }
    public void openDoor()
    {
        open = true;
    }

    public void closeDoor()
    {
        open = false;
    }
}
