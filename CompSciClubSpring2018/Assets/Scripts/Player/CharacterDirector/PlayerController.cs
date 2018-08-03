using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Physics")]
    public float gravity;
    public float coyoteTime;

    [Header("External")]
    public LayerMask excludePlayer;
    public LayerMask ground;
    public bool posFlag;


    private Transform playerTransform;
    private Vector3 checkBoxCenter;
    private Collider colliderBox;
    private Vector3 bRC;
    private Vector3 uRC;
    private Vector3 bLC;
    private Vector3 uLC;
    private Vector3 lBRC;
    private Vector3 lBLC;
    private Quaternion playerRotation;
    public bool isGrounded;
    private float currGravity;

    private float currSideMovement;
    public float maxRayDistance = 2.1f; // used in side collision; makes sure cross rays don't go above and below character.

    private Vector3 jumpVector;
    private Vector3 strafeVector;
    private float jumpForce;
    public float velocity = 2;

    void Start()
    {
        posFlag = false;
        playerTransform = this.gameObject.transform;
        colliderBox = this.gameObject.GetComponent<BoxCollider>();
    }

    void Update()
    {        
        bRC = colliderBox.bounds.center + new Vector3(colliderBox.bounds.extents.x, -colliderBox.bounds.extents.y, 0f);
        uRC = colliderBox.bounds.center + new Vector3(colliderBox.bounds.extents.x, colliderBox.bounds.extents.y, 0f);
        bLC = colliderBox.bounds.center + new Vector3(-colliderBox.bounds.extents.x, -colliderBox.bounds.extents.y, 0f);
        uLC = colliderBox.bounds.center + new Vector3(-colliderBox.bounds.extents.x, colliderBox.bounds.extents.y, 0f);
        lBRC = colliderBox.bounds.center + new Vector3(colliderBox.bounds.extents.x, -colliderBox.bounds.extents.y, 0f) - playerTransform.position;
        lBLC = colliderBox.bounds.center + new Vector3(-colliderBox.bounds.extents.x, -colliderBox.bounds.extents.y, 0f) - playerTransform.position;
        Move();
        //Gravity(false);
        //jump();
        //sideways();
    }

    public void Move()
    {
        Gravity(false);
        MoveRight(false);
        MoveLeft(false);
        jumpVector = new Vector3(0f, currGravity * Time.deltaTime, 0f);
        strafeVector = new Vector3(currSideMovement * Time.deltaTime, 0f, 0f);  // currSideMovement is changed in the MoveRight() and MoveLeft() functions
        playerTransform.position += (jumpVector);
    }

    public void Gravity(bool jump)
    {
        Vector3 groundPos = IsGrounded();
        

        if (isGrounded)
        {
            currGravity = 0f;
            if (!posFlag)
            {
                playerTransform.position = new Vector3(playerTransform.position.x, groundPos.y + colliderBox.bounds.extents.y, 0f);
                posFlag = true;
            }

            if (jump)
            {
                currGravity = jumpForce * Time.deltaTime;
            }
        }
        else
        {
            posFlag = false;
            currGravity += -gravity * Time.deltaTime; // set up value for terminal velocity
        }
    }

    public Vector3 IsGrounded()
    {
        Ray rightDown = new Ray(uRC, Vector3.down); //Right side of 'ground checker'
        RaycastHit hitRightDown;

        Ray leftDown = new Ray(uLC, Vector3.down); //Left Side of 'ground checker'
        RaycastHit hitLeftDown;

        if (Physics.Raycast(rightDown, out hitRightDown, (uRC - bRC).magnitude, ground))
        {
            Debug.DrawLine(uRC, bRC, Color.red);
            Debug.Log("Player is Grounded");
            isGrounded = true;
            return hitRightDown.point;
        }
        else if (Physics.Raycast(leftDown, out hitLeftDown, (uLC - bLC).magnitude, ground))
        {
            Debug.DrawLine(uLC, bLC, Color.red);
            Debug.Log("Player is Grounded");
            isGrounded = true;
            return hitLeftDown.point;
        }
        else
        {
            isGrounded = false;
            return Vector3.zero;
        }
    }


    public void MoveRight(bool isMovingRight)   //Same methods from the Inputlistener script
    {
        currSideMovement = 0f;

        if (isMovingRight)
        {
            currSideMovement = velocity; //currSideMovement will be added to the strafeVector in the Move() function to determine sideways movement
        }
    }

    public void MoveLeft(bool isMovingLeft)
    {
        currSideMovement = 0f;

        if (isMovingLeft && currSideMovement <= 0)  // You cant be moving left and right at the same time
        {
            currSideMovement = -velocity;
        }
    }

    public Vector3 SideCollision() //Incomplete function
    {
        // Crossing hit detection rays similar to 'ground checker' from above
        Ray diagonalDownRight = new Ray(uRC, new Vector3(.1F,-2.2F,0)); // top right corner to the bottom right; Values are dependant on the character size
        RaycastHit hitDiagonalDownRight;

        Ray diagonalUpRight = new Ray(bRC, new Vector3(.1F, 2.2F, 0)); //  bottom right corner to the top right
        RaycastHit hitDiagonalUpRight;

        Ray diagonalDownLeft = new Ray(uLC, new Vector3(-.1F, 2.2F, 0)); //  top left corner to the bottom left
        RaycastHit hitDiagonalDownLeft;

        Ray diagonalUpLeft = new Ray(bLC, new Vector3(-.1F, -2.2F, 0)); //  bottom left corner to the top left
        RaycastHit hitDiagonalUpLeft;

        Ray upRight = new Ray(uLC, Vector3.right); //from left to right
        RaycastHit hitUpRight;

        Ray downRight = new Ray(bLC, Vector3.right); //from left to right
        RaycastHit hitDownRight;

        Ray upLeft = new Ray(uRC, Vector3.left); //from right to left
        RaycastHit hitUpLeft;

        Ray downLeft = new Ray(uLC, Vector3.left); //from right to left
        RaycastHit hitDownLeft;

        if (Physics.Raycast(diagonalDownRight, out hitDiagonalDownRight, maxRayDistance, ground) ||
            Physics.Raycast(diagonalUpRight, out hitDiagonalUpRight, maxRayDistance, ground))
        {
            Debug.Log("Player is touching a wall");
            return hitDiagonalDownRight.point + playerTransform.position;
        }

        if (Physics.Raycast(diagonalDownLeft, out hitDiagonalDownLeft, maxRayDistance, ground) ||
            Physics.Raycast(diagonalUpLeft, out hitDiagonalUpLeft, maxRayDistance, ground))
        {
            Debug.Log("Player is touching a wall");
            return hitDiagonalDownRight.point + playerTransform.position;
        }

        if (Physics.Raycast(upRight, out hitUpRight, .5f, ground) ||
            Physics.Raycast(downRight, out hitDownRight, 5f, ground))
        {
            Debug.Log("Player is touching a wall");
            return hitDiagonalDownRight.point + playerTransform.position;
        }

        if (Physics.Raycast(upLeft, out hitUpLeft, .5f, ground) ||
            Physics.Raycast(downLeft, out hitDownLeft, 5f, ground))
        {
            Debug.Log("Player is touching a wall");
            return hitDiagonalDownRight.point + playerTransform.position;
        }

        else
        {
            return Vector3.zero;
        }
    }
}
