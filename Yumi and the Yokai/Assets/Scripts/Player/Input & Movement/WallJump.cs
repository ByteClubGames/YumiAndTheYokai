///*
//********************************************************************************
//*Creator(s).........................................................Kieran Glynn
//*Created..............................................................12/19/2018
//*Last Modified............................................@ 1:37PM on 12/21/2018
//*Last Modified by...................................................Keiran Glynn
//*
//*Description:   This script holds some static methods used for the wall jump
//*               mechanics of the player controller.
//********************************************************************************
//*/
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class WallJump : MonoBehaviour {

//    /// <summary>
//    /// Method to be called every frame to check if the player should still be sliding down a wall (alleviates issue where player would still be in sliding 
//    /// state after moving below the point of contact with the wall.
//    /// </summary>
//    /// <param name="playerController"> The instance of PlayerController.cs that this method is called from.</param>
//    /// <param name="slidingRight"> Bool for if the player is sliding on a wall to their right.</param>
//    /// <param name="boxCollider"> Box collider for the character being controlled.</param>
//    /// <param name="isGrounded"></param>
//    /// <param name="platformMask"> Layermask being used to check for normal ground and wall collisions.</param>
//    /// <returns></returns>
//    public static bool StillOnWall(PlayerController playerController, bool slidingRight, BoxCollider boxCollider, bool isGrounded, LayerMask platformMask)
//    {
//        Vector3 ray = new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.center.y, 0f);
//        float rayLength = boxCollider.bounds.extents.x + 0.01f;
//        /*This method should only be called when the player is sliding on a wall, so it is sufficient to base the 
//         * check direction only off of wheather they are sliding on a rightward wall */
//        Vector3 rayDirection = slidingRight ? Vector3.right : Vector3.left;

//        RaycastHit hit;

//        Debug.DrawRay(ray, rayDirection * rayLength, Color.yellow);

//        bool raycastHit = Physics.Raycast(ray, rayDirection, out hit, rayLength, platformMask);

//        if (raycastHit)
//        {
//            if ((hit.transform.gameObject.tag == "WallJump") && !isGrounded)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        else
//        {
//            return false;
//        }
//    }
//}
