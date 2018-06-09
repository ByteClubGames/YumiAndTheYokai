/*
 * Programmer:   Hunter Goodin 
 * Date Created: 03/21/2018 @ 10:00 PM 
 * Last Updated: 03/22/2018 @  7:00 PM 
 * File Name:    TouchInput.cs 
 * Description:  This script will be responsible for translating a finger's touch into a touch input. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    public LayerMask touchInputMask;
    public List<GameObject> touchList = new List<GameObject>();
    public GameObject[] touchesOld;
    private RaycastHit hit;
    public GameObject leftMover;
    public GameObject rightMover;

    public GameObject recipient; 

    // I havent attached this script to the main camera yet 
    // so we might need to go back and delete the cam variable later on
    //private Camera cam;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            touchesOld = new GameObject[touchList.Count];
            touchList.CopyTo(touchesOld);
            touchList.Clear();
        }

        // The foreach statement is used to iterate through the collection
        // to get the information that you want, but can not be used to
        // add or remove items from the source collection to avoid unpredictable side effects

        foreach (Touch touch in Input.touches)
        {
            // Takes a point on the screen and transforms it so 
            // the ray goes from the cameras position to the touched position
            // and sees what objects the ray is colliding with

            Ray ray = GetComponent<Camera>().ScreenPointToRay(touch.position);

            // only checks the touched layer and not the entire Unity scene with variable touchInputMask
            if (Physics.Raycast(ray, out hit, touchInputMask))
            {
                // if the ray hits a game object
                recipient = hit.transform.gameObject;
                touchList.Add(recipient);

                // The conditions below check the current status of the players touch input

                if (touch.phase == TouchPhase.Began)
                {
                    if (recipient.name == "MovePlayerLeft")
                    {
                        recipient.SendMessage("Move", hit.point, SendMessageOptions.DontRequireReceiver);
                        rightMover.SetActive(false); 
                    }
                    if (recipient.name == "MovePlayerRight")
                    {
                        recipient.SendMessage("Move", hit.point, SendMessageOptions.DontRequireReceiver);
                        leftMover.SetActive(false);
                    }
                    if (recipient.name == "MovePlayerJump")
                    {
                        recipient.SendMessage("MakeJumpTrue", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    recipient.SendMessage("StopMoving", hit.point, SendMessageOptions.DontRequireReceiver);
                    recipient.SendMessage("MakeJumpFalse", hit.point, SendMessageOptions.DontRequireReceiver);
                    leftMover.SetActive(true);
                    rightMover.SetActive(true);
                }

                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    recipient.SendMessage("Move", hit.point, SendMessageOptions.DontRequireReceiver);
                }

                if (touch.phase == TouchPhase.Canceled)
                {
                    recipient.SendMessage("StopMoving", hit.point, SendMessageOptions.DontRequireReceiver);
                    recipient.SendMessage("MakeJumpFalse", hit.point, SendMessageOptions.DontRequireReceiver);
                    leftMover.SetActive(true);
                    rightMover.SetActive(true);
                }
            }

            // If the players touch input doesnt match any of the conditions above,
            // the code below will run so all possible touch inputs will be checked. 
            foreach (GameObject g in touchesOld)
            {
                if (!touchList.Contains(g))
                {
                    g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }
}
