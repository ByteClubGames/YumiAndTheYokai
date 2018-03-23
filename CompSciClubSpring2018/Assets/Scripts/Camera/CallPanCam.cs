using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Programmer: Benjamin Riviera
 * Description: This is a Java Class.
 */
public class CallPanCam : MonoBehaviour {
    public Camera exampleCamera;

     void Start()
    {
        exampleCamera.GetComponent<CameraSwitch>().ShowPanCamera();
    }
    void Update ()
    {
       exampleCamera.GetComponent<CameraMove>().CameraMovement(); 	
	}

}
