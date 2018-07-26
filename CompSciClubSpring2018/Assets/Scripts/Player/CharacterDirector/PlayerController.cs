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


    private Transform playerTransform;
    private Vector3 checkBoxCenter;
    private Collider colliderBox;
    private Vector3 bRC;
    private Vector3 bLC;
    private Vector3 lBRC;
    private Vector3 lBLC;
    private Quaternion playerRotation;
    public bool isGrounded;
    private bool currGravity;

    private Vector3 movementVector;
    private float velocity;

    void Start()
    {
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
        //OnDrawGizmos();
        SetIsGrounded();
        //gravity();
        //jump();
        //sideways();
    }

    public void SetIsGrounded()
    {
        Ray rightDown = new Ray(bRC, Vector3.down); //Right side of 'ground checker'
        RaycastHit hitRightDown;

        Ray leftDown = new Ray(bLC, Vector3.down); //Left Side of 'ground checker'
        RaycastHit hitLeftDown;

        if (Physics.Raycast(rightDown, out hitRightDown, 0.1f) || Physics.Raycast(leftDown, out hitLeftDown, 0.1f))
        {
            Debug.Log("Player is Grounded");
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(playerTransform.position, Vector3.one);
    }
















}
