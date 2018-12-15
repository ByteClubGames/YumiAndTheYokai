/*
*
*Creator(s).........................................................Brenden Plong
*Created..............................................................10/05/2018
*Last Modified............................................@ 4:30PM on 10/19/2018
*Last Modified by...................................................Brenden Plong
*
*Description:   Modular script that is used to destroy objects based on collisions
**
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnColDestroy : MonoBehaviour {
    // public GameObject player; // Used for grabbing the "player" object
    // public GameObject enemy; // Used for grabbing the "enemy" object
    // public LayerMask foe;
    // public LayerMask friend;
    // (other.gameObject.layer == 9)
    private float originOffset = 0.5f;
    public float raycastMaxDistance = 10f;
    
    /*
    public void OnTriggerEnter(Collider other) // Deals with the destruction of enemies
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) // Checks if the layer is under the Player layer
        {
            //Destroy(this.gameObject); // Destroys the gameObject
            Debug.Log("Has Collided"); // Tells if there was a collision
        }

    }*/

    /*
    private void rayCaster()
    {
        RaycastHit hit;
        Vector3 forward = new Vector3(transform.position.x, 0f, 0f);
        Debug.DrawRay(transform.position, forward, Color.green);
    }
    */
/*
    private RaycastHit2D CheckRayCast(Vector2 direction)
    {
        float directionOriginOffset = originOffset * (direction.x > 0 ? 1 : -1);

        Vector2 startingposition = new Vector2(transform.position.x + directionOriginOffset, transform.position.y);

        Debug.DrawRay(startingposition, direction, Color.green);
        return Physics2D.Raycast(startingposition, direction, raycastMaxDistance);
    }

    private bool RayCastCheck(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) {

            Vector2 direction = new Vector2(1, 0);

            RaycastHit2D hit = CheckRayCast(direction);

            if (hit.collider)
                Debug.Log("Hit the collidable object " + hit.collider.name);

            return true;
        }
        else
        {
            return false;
        }
    }
    */
    //-----------------------------------------------------------------------------------------------

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) // Checks if the layer is under the Player layer
        {
            Destroy(this.gameObject); // Destroys the gameObject
            Destroy(other.gameObject); // Destorys the player object
            //Debug.Log("Has Collided"); // Tells if there was a collision
            Debug.Log(ReturnDirection(this.gameObject, other.gameObject)); // Gives the side on which the obj collided into
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall")) // Checks if the layer is under the Wall layer
        {
            Destroy(this.gameObject); // Destroys the gameobject
            Debug.Log(ReturnDirection(this.gameObject, other.gameObject)); // Gives the side on which the obj collided into
        }
        
    }
    /*
    public bool isMoving(Collider other)
    {
        if(this.gameObject.transform == other.gameObject)
        {
            OnTriggerEnter(other);
            return true;
        }
        else
        {
            return false;
        }
    }
    */
    
    // This block of code is used to check which sides are collided into
    // Given from a tutorial, as in the wise words of Todd Howard, "it just works."
    private enum HitDirection { None, Top, Bottom, Forward, Back, Left, Right }
    private HitDirection ReturnDirection(GameObject Object, GameObject ObjectHit)
    {

        HitDirection hitDirection = HitDirection.None;
        RaycastHit MyRayHit;
        Vector3 direction = (Object.transform.position - ObjectHit.transform.position).normalized;
        Ray MyRay = new Ray(ObjectHit.transform.position, direction);
        
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
}


