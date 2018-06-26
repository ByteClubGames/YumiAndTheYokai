
// *** R E D U N D A N T  S C R I P T ***  This script is replaced by the SetIsGrounded method in the HumanJump.cs class. HumanJump.cs will soon be modularized, making this script 100% obsolete - Keiran

///*
// * 
// * Author: Spencer Wilson, Hunter Goodin, Keiran Glynn
// * Date Created:  03/23/2018 @ 11:59 AM
// * Last Modified: 06/25/2018 @  6:20 PM
// * Project: CompSciClubSpring2018
// * File: GroundCheck.cs
// * Description: Checks if the player is colliding with the ground or not.
// * 
// */

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GroundCheck : MonoBehaviour
//{
//    public GameObject button; // To be populated with the jump button in engine 
//    public bool isColliding;

//    private void OnTriggerEnter(Collider collision)
//    {
//        if (collision.gameObject.tag == "Platforms")
//        {
//            if (button.gameObject.name == "MovePlayerJump")
//            {
//                isColliding = true;
//                // button.GetComponent<HumanJump>().SetIsGrounded(true);
//                // character.GetComponent<HumanMovement>().SetIsGrounded(true);
//            }
//            else if (button.gameObject.name == "Ferrox")
//            {
//                isColliding = true;
//                // button.GetComponent<FerroxJump>().SetIsGrounded(true);
//                //character.GetComponent<FerroxMovement>().SetIsGrounded(true);
//            }
//        }
//    }

//    private void OnTriggerExit(Collider collision)
//    {
//        if (collision.gameObject.tag == "Platforms")
//        {
//            if (button.gameObject.name == "MovePlayerJump")
//            {
//                isColliding = false;
//                // button.GetComponent<HumanJump>().SetIsGrounded(false);
//                // character.GetComponent<HumanMovement>().SetIsGrounded(false);
//            }
//            else if (button.gameObject.name == "Ferrox")
//            {
//                isColliding = false;
//                // button.GetComponent<FerroxJump>().SetIsGrounded(false);
//                // character.GetComponent<FerroxMovement>().SetIsGrounded(false);
//            }
//        }
//    }

//    public bool GetIsColliding()
//    {
//        return isColliding; 
//    }
//}
