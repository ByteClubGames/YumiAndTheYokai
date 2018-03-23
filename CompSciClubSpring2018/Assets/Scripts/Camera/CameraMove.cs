using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* 
 * 
 * Author: Karim Dabboussi
 * Date Created: 03/16/2018 @ 12:50 pm
 * Date Modified: 03/16/2018 @ 12:50 pm
 * Project: CompSciClubSpring2018
 * File: CameraSwitch.cs
 * Description: This script moves a camera from its current position to a NEW POSITION(LocCoords) to Another Position { cameraObject - > A - > cameraObject }
 *
 */
public class CameraMove : MonoBehaviour
{
    public Vector3 localPosition;
    public float smoothTime = 0.001f; //this is what controls the speed, edit it from unity

    public Transform newLoc;
    public Transform orgCoords;
    private Vector3 locCoords;
    private Vector3 originCoords;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {

        originCoords = orgCoords.localPosition; // The vector is assigned the value of the transform of the orgCoords
        locCoords = newLoc.position; // the vector is assigned the value of the locCoords.
    }


    public void CameraMovement()
    {
        StartCoroutine(Wait(15)); // this is how you get an action to wait, stuff that waits must be declared in the Inumerator
    }
    private IEnumerator Wait(float waitDuration)
    {
        float maxSpeed = Mathf.Infinity; // additional components that help in smoothing, dont modify
        float deltaTime = Time.deltaTime; // additional components that help in smoothing, dont modify makes it glitchy
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, locCoords, ref velocity, smoothTime, maxSpeed, deltaTime); // Camera to First Position
        yield return new WaitForSeconds(waitDuration); // This is what causes script to wait
        transform.localPosition = Vector3.SmoothDamp(orgCoords.transform.localPosition, originCoords,ref velocity, smoothTime, maxSpeed, deltaTime); // First Position to Camera
    }
}
//void Start() {
//    panCamera.transform.localPosition = new Vector3(0, 13, -31);
//}

//// Update is called once per frame
//void Update() {
//    transform.localPosition = Vector3.Lerp(transform.localPosition, end, Time.deltaTime * speed);
//}