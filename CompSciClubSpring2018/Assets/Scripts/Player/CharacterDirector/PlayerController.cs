/*
 *  Authors: Kieran Glynn, Darrell Wong
 *  Date Created: 7/26/2018
 *  Last Modified: 8/3/2018
 *  PlayerController.cs
 *  Description: This script is the definition of the players physics. It handles:
 *      1. Movement (Jump and Strafe)
 *      2. Collisions using raycasts
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Physics")]
    public float gravity;
    public float coyoteTime;
    public float terminalSpeed;
    public float buffer = .1f;

    [Header("External")]
    public LayerMask excludePlayer;
    public LayerMask ground;
    public bool verticalFlag;
    private bool jump;
    private bool canJump;
    private bool isTouchingWall;
   

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
    public bool isBelow;
    private float currGravity;

    private float currSideMovement;
    public float maxClimbableSlope = 70; //in degrees
    private Vector3 verticalVector;
    private Vector3 strafeVector;
    public float jumpSpeed = 10;
    public float velocity = 5;
    public bool moveX;

    void Start()
    {
        jump = false;
        terminalSpeed = 4;
        verticalFlag = false;
        playerTransform = this.gameObject.transform;
        colliderBox = this.gameObject.GetComponent<BoxCollider>();
        verticalVector = Vector3.zero;
    }

    void Update()
    {
        // Used to identify each corner of the Box Collider around the player
        #region BoxCollider Initialization
        bRC = colliderBox.bounds.center + new Vector3(colliderBox.bounds.extents.x, -colliderBox.bounds.extents.y, 0f);
        uRC = colliderBox.bounds.center + new Vector3(colliderBox.bounds.extents.x, colliderBox.bounds.extents.y, 0f);
        bLC = colliderBox.bounds.center + new Vector3(-colliderBox.bounds.extents.x, -colliderBox.bounds.extents.y, 0f);
        uLC = colliderBox.bounds.center + new Vector3(-colliderBox.bounds.extents.x, colliderBox.bounds.extents.y, 0f);
        lBRC = colliderBox.bounds.center + new Vector3(colliderBox.bounds.extents.x, -colliderBox.bounds.extents.y, 0f) - playerTransform.position;
        lBLC = colliderBox.bounds.center + new Vector3(-colliderBox.bounds.extents.x, -colliderBox.bounds.extents.y, 0f) - playerTransform.position;
        #endregion


        // Check if Player is colliding anywhere and return the position vector of that collision
        Vector3 ceilingPos = IsBelow();
        Vector3 groundPos = IsGrounded();
        Vector3 sidePos = SideCollision();

        if (!moveX)
        {
            strafeVector = Vector3.zero;
        }

        if (sidePos != Vector3.zero)
        {
            isTouchingWall = true;
            CollidingSide(sidePos);
        }
        else
        {
            isTouchingWall = false;
        }

        if (jump && canJump)
        {
            canJump = false;
            verticalVector = new Vector3(0f, jumpSpeed * Time.deltaTime, 0f);
            jump = false;
            Debug.Log("D");
        }

        else if ((ceilingPos != Vector3.zero) && (groundPos != Vector3.zero))
        {
            /* This case would occur if the player was hitting the floor and the roof at the same exact time (most likely occuring if the player walks on a path that is
             * too narrow for its height). This is not immediately important so it will be ignored, however it will need to be addressed soon as it could lead to gamebreaking
             * bugs */
            canJump = false;
            verticalVector = Vector3.zero;
            jump = false;
            Debug.Log("A");
        }
        else if (ceilingPos != Vector3.zero) // if colliding with ground above the player
        {
            CollidingTop(ceilingPos);
            verticalFlag = false;
            canJump = false;
            verticalVector = Vector3.zero;
            jump = false;
            Debug.Log("B");
        }
        else if (groundPos != Vector3.zero) // if colliding with ground below the player
        {
            CollidingBottom(groundPos);
            verticalFlag = false;
            canJump = true;
            verticalVector = Vector3.zero;
            jump = false;
            Debug.Log("C");
        }
        else
        {
            if ((verticalVector.y < terminalSpeed) && (canJump != true) || groundPos == Vector3.zero || ceilingPos != Vector3.zero)
            {
                verticalVector += new Vector3(0f, -gravity * Time.deltaTime, 0f);
                jump = false;
                Debug.Log("E");
            }
        }


        Move(verticalVector, strafeVector);
        moveX = false;
        

        //Move();
        //Gravity(false);
        //jump();
        //sideways();
    }

    public void Move(Vector3 vertical, Vector3 horizontal)
    {
        Vector3 sumVector = vertical + horizontal;
        playerTransform.position += sumVector;
    }

    public void TempStrafe(bool leftRight)
    {
        ;

        if (leftRight)
        {
            strafeVector = new Vector3(velocity * Time.deltaTime, 0f, 0f);
        }
        else
        {
            strafeVector = new Vector3(-velocity * Time.deltaTime, 0f, 0f);
        }        
    }

    public void Jump()
    {
        jump = true;
        //jump = false;
    }

    public void CollidingBottom(Vector3 pos)
    {
        if (!verticalFlag)
        {
            playerTransform.position = new Vector3(playerTransform.position.x, pos.y + colliderBox.bounds.extents.y, 0f);
            verticalFlag = true;
        }
    }

    public void CollidingTop(Vector3 pos)
    {
        if (!verticalFlag)
        {
            playerTransform.position = new Vector3(playerTransform.position.x, pos.y - colliderBox.bounds.extents.y, 0f);
            verticalFlag = true;
        }
    }

    public void CollidingSide(Vector3 pos) //transform position for side collisions
    {
        if (pos.x > playerTransform.position.x)
        {
            playerTransform.position = new Vector3(pos.x - colliderBox.bounds.extents.x - .1f, playerTransform.position.y, 0f);
        }
        if (pos.x < playerTransform.position.x)
        {
            playerTransform.position = new Vector3(pos.x + colliderBox.bounds.extents.x + .1f, playerTransform.position.y, 0f);
        }
    }

    // Method that returns the Vector3 positioin of the floor platform below the player. If no floor is detected, then it returns Vector3.zero
    public Vector3 IsGrounded()
    {
        Ray rightDown = new Ray(uRC, Vector3.down); //Right side of 'ground checker'
        RaycastHit hitRightDown;

        Ray leftDown = new Ray(uLC, Vector3.down); //Left Side of 'ground checker'
        RaycastHit hitLeftDown;

        // if the ground is detected anywhere along the path from the upper right-hand corner of the box to the lower right-hand corner of the box:
        if (Physics.Raycast(rightDown, out hitRightDown, (uRC - bRC).magnitude, ground))
        {
            if(hitRightDown.normal.y > 0f) // if the detected "ground" surface was below the player:
            {
                Debug.DrawLine(uRC, bRC, Color.red);
                Debug.Log("Player is Grounded");
                isGrounded = true;
                return hitRightDown.point;
            }
            else // The roof was detected
            {
                isGrounded = false;
                return Vector3.zero;
            }
        }
        // if the ground is detected anywhere along the path from the upper left-hand corner of the box to the lower left-hand corner of the box:
        else if (Physics.Raycast(leftDown, out hitLeftDown, (uLC - bLC).magnitude, ground))
        {
            if(hitLeftDown.normal.y > 0f) // if the detected "ground" surface was below the player: 
            {
                Debug.DrawLine(uLC, bLC, Color.red);
                Debug.Log("Player is Grounded");
                isGrounded = true;
                return hitLeftDown.point;
            }
            else // The roof was detected
            {
                isGrounded = false;
                return Vector3.zero;
            }            
        }
        else // Case in which the player probably isn't grounded
        {
            isGrounded = false;
            return Vector3.zero;
        }
    }

    // Method that returns the Vector3 positioin of the roof platform above the player. If no roof is detected, then it returns Vector3.zero
    public Vector3 IsBelow()
    {
        Ray rightUp = new Ray(bRC, Vector3.up); //Right side of 'ceiling checker'
        RaycastHit hitRightUp;

        Ray leftUp = new Ray(bLC, Vector3.up); //Left Side of 'ceiling checker'
        RaycastHit hitLeftUp;

        // if the ground is detected anywhere along the path from the lower right-hand corner of the box to the upper right-hand corner of the box:
        if (Physics.Raycast(rightUp, out hitRightUp, (uRC - bRC).magnitude, ground))
        {
            if (hitRightUp.normal.y < 0f) // if the detected "roof" surface was above the player:
            {
                Debug.DrawLine(bRC, uRC, Color.magenta);
                Debug.Log("Player is colliding with ceiling");
                isBelow = true;
                return hitRightUp.point;
            }
            else // The floor was detected
            {
                isBelow = false;
                return Vector3.zero;
            }
        }
        // if the roof is detected anywhere along the path from the lower left-hand corner of the box to the upper left-hand corner of the box:
        else if (Physics.Raycast(leftUp, out hitLeftUp, (uLC - bLC).magnitude, ground))
        {
            if (hitLeftUp.normal.y < 0f) // if the detected "roof" surface was above the player: 
            {
                Debug.DrawLine(bLC, uLC, Color.magenta);
                Debug.Log("Player is colliding with ceiling");
                isBelow = true;
                return hitLeftUp.point;
            }
            else // The floor was detected
            {
                isBelow = false;
                return Vector3.zero;
            }
        }
        else // Case in which the player probably isn't hitting the ceiling
        {
            isBelow = false;
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
        Ray upRight = new Ray(uLC, Vector3.right); //from left to right
        RaycastHit hitUpRight;

        Ray downRight = new Ray(bLC + new Vector3(0f, .1f, 0f), Vector3.right); //from left to right
        RaycastHit hitDownRight;

        Ray upLeft = new Ray(uRC, Vector3.left); //from right to left
        RaycastHit hitUpLeft;

        Ray downLeft = new Ray(bRC + new Vector3(0f, .1f, 0f), Vector3.left); //from right to left
        RaycastHit hitDownLeft;


        //  Right side of  collision detections
        if (Physics.Raycast(upRight, out hitUpRight, (uRC - uLC).magnitude + .1f, ground))
        {
            //print(Mathf.Atan2(hitUpRight.normal.y, hitUpRight.normal.x) * Mathf.Rad2Deg +" uR");
            
            if (Mathf.Atan2(hitUpRight.normal.y, hitUpRight.normal.x) * Mathf.Rad2Deg > maxClimbableSlope + 90) //This if statement checks angle of the normal to make sure it is a climable slope
            {
                Debug.DrawLine(uLC, uRC, Color.blue);
                Debug.Log("Player is Touching wall up right");
                return hitUpRight.point;
            }
            else
            {
                return Vector3.zero;
            }
        }
        else if (Physics.Raycast(downRight, out hitDownRight, (bRC - bLC).magnitude + .1f, ground))
        {
            print(Mathf.Atan2(hitDownRight.normal.y, hitDownRight.normal.x) * Mathf.Rad2Deg + " dR" );

            if (Mathf.Atan2(hitDownRight.normal.y, hitDownRight.normal.x) * Mathf.Rad2Deg > maxClimbableSlope + 90)
            {
                Debug.DrawLine(bLC, bRC, Color.blue);
                Debug.Log("Player is Touching wall down right");
                return hitDownRight.point;
            }
            else
            {
                return Vector3.zero;
            }
        }

        //Left side of collision detections
        if (Physics.Raycast(upLeft, out hitUpLeft, (uRC - uLC).magnitude + .1f, ground))
        {
            //print(Mathf.Atan2(hitDownRight.normal.x, hitDownRight.normal.y) * Mathf.Rad2Deg + " uL");

            if (Mathf.Atan2(hitUpLeft.normal.x, hitUpLeft.normal.y) * Mathf.Rad2Deg > maxClimbableSlope)
            {
                Debug.DrawLine(uLC, uRC, Color.blue);
                Debug.Log("Player is Touching wall up left");
                return hitUpLeft.point;
            }
            else
            {
                return Vector3.zero;
            }
        }
        else if (Physics.Raycast(downLeft, out hitDownLeft, (bRC - bLC).magnitude + .1f, ground))
        {
            //print(Mathf.Atan2(hitDownLeft.normal.x, hitDownLeft.normal.y) * Mathf.Rad2Deg + " dL");

            if (Mathf.Atan2(hitDownLeft.normal.x, hitDownLeft.normal.y) * Mathf.Rad2Deg > maxClimbableSlope)
            {
                Debug.DrawLine(bLC, bRC, Color.blue);
                Debug.Log("Player is Touching wall down left");
                return hitDownLeft.point;
            }
            else
            {
                return Vector3.zero;
            }
        }

        else
        {
            return Vector3.zero;
        }
    }
}
