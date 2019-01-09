/*
*
*Creator(s).........................................................Brenden Plong
*Created..............................................................10/05/2018
*Last Modified............................................@ 10:00AM on 1/5/2018
*Last Modified by...................................................Brenden Plong
*
*Description:   Modular script that is used to destroy objects based on collisions
**
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnColDestroy : MonoBehaviour {
    private float originOffset = 0.5f; // Used for the placement of the ray to be outside of the collider
    public float raycastMaxDistance = 10f;
    //-----------------------------------------------------------------------------------------------
    public void OnTriggerEnter(Collider other)
    {
        // This if statement will be used to identify player collisions
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))// Checks if the layer is under the Player layer
        {
            this.transform.DetachChildren(); // Detaches the child objects from parents so that they wont be destroyed
            Destroy(this.gameObject); // Destroys the gameObject the script is attached to
            Destroy(other.gameObject); // Destorys the player object
            //Debug.Log("Has Collided"); // Tells if there was a collision
            Debug.Log(ReturnDirection(this.gameObject, other.gameObject)); // Gives the side on which the obj collided into
        }
        //This if statement will be used to identify wall collisions
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall")) // Checks if the layer is under the Wall layer
        {
            this.transform.DetachChildren(); // Detaches the child objects from parents
            Destroy(this.gameObject); // Destroys the gameobject
            Debug.Log(ReturnDirection(this.gameObject, other.gameObject)); // Gives the side on which the obj collided into
        } 
    }
    //------------------------------------------------------------------------------------------------- 
    // This block of code is used to check which sides are collided into
    // Given from a tutorial, as in the wise words of Todd Howard, "it just works."
    private enum HitDirection { // Sets up the cardinal directions
        None, Top, Bottom, Forward, Back, Left, Right
    }

    private HitDirection ReturnDirection(GameObject Object, GameObject ObjectHit) // Will tell the programmer which side is collided into
    {
        HitDirection hitDirection = HitDirection.None; // Sets up the hitDirection as none
        RaycastHit MyRayHit; // Initialize raycast
        Vector3 direction = (Object.transform.position - ObjectHit.transform.position).normalized;
        Ray MyRay = new Ray(ObjectHit.transform.position, direction); // Will take the ObjectHit's positiion and direction
        if (Physics.Raycast(MyRay, out MyRayHit))
        {      
            if (MyRayHit.collider != null)
            {
                Vector3 MyNormal = MyRayHit.normal;
                MyNormal = MyRayHit.transform.TransformDirection(MyNormal);

                if (MyNormal == MyRayHit.transform.up){hitDirection = HitDirection.Top;}
                if (MyNormal == -MyRayHit.transform.up){hitDirection = HitDirection.Bottom;}
                if (MyNormal == MyRayHit.transform.forward){ hitDirection = HitDirection.Forward;}
                if (MyNormal == -MyRayHit.transform.forward){hitDirection = HitDirection.Back;}
                if (MyNormal == MyRayHit.transform.right){ hitDirection = HitDirection.Right;}
                if (MyNormal == -MyRayHit.transform.right){hitDirection = HitDirection.Left;}
            }
        }
        return hitDirection;
    }
    // -------------------------------------------------------------------------------------------------------------------------
}


