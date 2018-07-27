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

    private Vector3 jumpVector;
    private Vector3 strafeVector;
    private float jumpForce;
    private float velocity;

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
        jumpVector = new Vector3(0f, currGravity * Time.deltaTime, 0f);
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
                playerTransform.position = new Vector3(playerTransform.position.x, groundPos.y - 1.5f * colliderBox.bounds.extents.y, 0f);
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
        Ray rightDown = new Ray(bRC, Vector3.down); //Right side of 'ground checker'
        RaycastHit hitRightDown;

        Ray leftDown = new Ray(bLC, Vector3.down); //Left Side of 'ground checker'
        RaycastHit hitLeftDown;

        if (Physics.Raycast(rightDown, out hitRightDown, 0.5f, ground))
        {
            Debug.Log("Player is Grounded");
            isGrounded = true;
            return hitRightDown.point + playerTransform.position;
        }
        else if (Physics.Raycast(leftDown, out hitLeftDown, 0.5f, ground))
        {
            Debug.Log("Player is Grounded");
            isGrounded = true;
            return hitLeftDown.point + playerTransform.position;
        }
        else
        {
            isGrounded = false;
            return Vector3.zero;
        }
    }
}
