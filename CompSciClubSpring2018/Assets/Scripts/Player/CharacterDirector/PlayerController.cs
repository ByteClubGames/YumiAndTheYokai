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
    private Vector3 bLC;
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
        //colliderCenter = colliderBox.bounds.center;
        //bottomRightCorner =
        //playerRotation = this.gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("min: " + colliderBox.bounds.min);
        //Debug.Log("max: " + colliderBox.bounds.max);
        //Debug.Log("x: " + colliderBox.bounds.extents.x);
        //Debug.Log("y: " + colliderBox.bounds.extents.y);
        //Debug.Log("z: " + colliderBox.bounds.extents.z);
        //Debug.Log("center: " + colliderBox.bounds.center);
        //Debug.Log("R: " + lBRC);
        //Debug.Log("L: " + lBLC);
        //Debug.Log("PTP: " + playerTransform.position);
        bRC = colliderBox.bounds.center + new Vector3(colliderBox.bounds.extents.x, -colliderBox.bounds.extents.y, 0f);
        bLC = colliderBox.bounds.center + new Vector3(-colliderBox.bounds.extents.x, -colliderBox.bounds.extents.y, 0f);
        lBRC = colliderBox.bounds.center + new Vector3(colliderBox.bounds.extents.x, -colliderBox.bounds.extents.y, 0f) - playerTransform.position;
        lBLC = colliderBox.bounds.center + new Vector3(-colliderBox.bounds.extents.x, -colliderBox.bounds.extents.y, 0f) - playerTransform.position;
        Move();
        //Gravity(false);
        //jump();
        //sideways();
    }

    private void FixedUpdate()
    {
        //Gravity(false);
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

            //return new Vector3(0f, currGravity * Time.deltaTime, 0f);
        }
        else
        {
            posFlag = false;
            currGravity += -gravity * Time.deltaTime; // set up value for terminal velocity
            //return new Vector3(0f, currGravity * Time.deltaTime, 0f);
        }

        //return new Vector3(0f, currGravity, 0f);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(playerTransform.position, Vector3.one);
    }
















}
