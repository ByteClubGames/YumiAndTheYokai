/*
 * Programmer:   Hunter Goodin 
 * Date Created: 03/22/2018 @  7:00 PM 
 * Last Updated: 03/22/2018 @ 10:00 PM 
 * File Name:    SwipeTest.cs 
 * Description:  This script will be responsible for maing the player character jump. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTest : MonoBehaviour
{
    public Swipe swipeControls;
    public Rigidbody player;
    public int jumpForce = 7;

    public bool touched;
    public bool isJump;

    public LayerMask groundLayers;
    public CapsuleCollider col;

    //public Transform cube1;
    //public Transform cube2;
    //public Transform cube3; 

    private Vector3 desiredPosition;

    private void Update()
    {
        //if (swipeControls.SwipeLeft)
        //{
        //    desiredPosition += Vector3.left;
        //}
        //if (swipeControls.SwipeRight)
        //{
        //    desiredPosition += Vector3.right;
        //}
        if (swipeControls.SwipeUp)
        {
            player.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        //if (swipeControls.SwipeDown)
        //{
        //    desiredPosition += Vector3.down;
        //}

        //player.transform.position = Vector3.MoveTowards(player.transform.position, desiredPosition, 3f * Time.deltaTime);

        if (swipeControls.Tap)
        {
            Debug.Log("Tap");
        }
    }

    private bool IsGrounded()
    {
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z), col.radius * .9f, groundLayers);
    }

    //public void playerChanger(GameObject rec)
    //{
    //    if (rec.name == cube1.name)
    //    {
    //        player = cube1; 
    //    }
    //    if (rec.name == cube2.name)
    //    {
    //        player = cube2;
    //    }
    //    if (rec.name == cube3.name)
    //    {
    //        player = cube3;
    //    }
    //}
}
