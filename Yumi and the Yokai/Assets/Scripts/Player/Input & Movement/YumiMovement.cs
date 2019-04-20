/*
********************************************************************************
*Creator(s)...........................................Keiran Glynn, Darrell Wong
*Created...............................................................7/26/2018
*Last Modified...........................................@ 12:56AM on 02/22/2019
*Last Modified by...................................................Keiran Glynn
*
*Description: This script translates the player in the direction and with the
* magnitude of a movement vector. This vector is refered to as the delta 
* movement for the character, as it is the small bit of distance that the player
* will be moving during the frame.
* 
* The delta movement for the player is calculated by collecting several proposed 
* movement vectors (does the player want to jump up? Is gravity moving them
* down? Are they walking right? Are they being pushed left?) during the frame. 
* All of these proposed movement vectors are added together to get the delta
* movement.
* 
* Once the deta movement is calculated, the vector is checked to make sure that
* it will not cause the player to move into a position in which it will collide
* with any walls, platforms, or the like.
********************************************************************************
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YumiMovement : MonoBehaviour
{
    [Header("Physics/ Player Attributes")]
    public float gravity = -25f; // Negative input value
    public float jumpSpeed = 3f;
    //public float headCheck = 1.1f;      // disables jumping when under a low ceiling, look for headCheckRaycast
    public float horizontalSpeed = 8f; // Movement speed along horizontal axis
    //public float wallSlideSpeed = 2f;
    //public float wallJumpXSpeed = 6f;
    //public float wallJumpYSpeed = 8f;
    //public float wallJumpXDistance = 1f;
    public float maxClimbableSlope = 50f; //in degrees
    public float fallBuffer = .2f; // Time in seconds before fall animation should play when falling
    public float skinWidth; // Acts as an inset start point on the collider for the Ray orgin points
    public float shortHopCoefficient = 2f;   //decides how snappy the shorthop will be

    [Header("Collision Detection")]
    [Range(2, 12)]
    public int horizontalRays = 8;
    [Range(2, 10)]
    public int verticalRays = 4;
    public float error = .01f;
    public LayerMask platformMask;

    private Vector3 velocity;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Transform yumi;

    //private List<Vector3> movementDeltas;
    private Queue movementDeltas = new Queue();

    // Don't reset deltaMovement.x every frame. 
    // Add a delta x for movement every frame, subtract a friction depending on the surface, and check agaisnt a speed limit.

    // Start is called before the first frame update
    void Start()
    {

    }

    Vector3 deltaMovement = Vector3.zero;

    // Update is called once per frame
    void FixedUpdate()
    {
        //deltaMovement.x = 0f;
        deltaMovement.y = 0f;

        deltaMovement = SumMovementDeltas(movementDeltas);
        // deltaMovement = AdjustDeltaMovement();
        Move(deltaMovement);
        velocity = CalculateVelocity(deltaMovement, velocity);
    }

    /// <summary>
    /// Function that adds a proposed movement delta to a queue. All movement deltas will be added together on the next frame, 
    /// adjusted for collisions, and then applied as movement to the player for that frame.
    /// </summary>
    /// <param name="new_Vector">The movement that you would like the character to perform during the next frame.</param>
    public void AddMovementDelta(Vector3 new_Vector)
    {
        movementDeltas.Enqueue(new_Vector);
    }

    /// <summary>
    /// Add a force to 
    /// </summary>
    /// <param name="force_vector"></param>
    public void AddInstantaneousForce(Vector3 force_vector)
    {

    }

    public void AddConstantForce(Vector3 force_vector)
    {

    }

    struct Movement_Velocity
    {
        private Vector3 velocity_Vector;
        //private float decay_Factor;
        //private bool is_Constant;

        private Movement_Velocity(Vector3 VelocityVector)
        {
            this.velocity_Vector = VelocityVector;
            //this.decay_Factor = decayFactor;
        }

        //private something that subratcs the decay factor from the vector to bring it towards zero.


    }

    /// <summary>
    /// Gets rid of all the proposed movement deltas for the next frame. Only use this if you do not want the character to move next frame.
    /// </summary>
    public void ClearMovementQueue()
    {
        movementDeltas.Clear();
    }

    /// <summary>
    /// Adds up every proposed movement delta for the next frame into a single vector that can then be applied to the character.
    /// </summary>
    /// <param name="movement_Deltas">This is the queue containing every proposed movement delta for the next frame.</param>
    /// <returns>Returns the sum of all the movement deltas.</returns>
    private Vector3 SumMovementDeltas(Queue movement_Deltas)
    {
        Vector3 total = Vector3.zero;

        if (movement_Deltas.Count != 0)
        {
            for (int i = 0; i < movement_Deltas.Count; i++)
            {
                total += (Vector3)movement_Deltas.Dequeue();
            }
        }

        return total;
    }

    /// <summary>
    /// Applies a movement vector to the player.
    /// </summary>
    /// <param name="delta_Movement">The small amount of movement the player is going to make this frame.</param>
    private void Move(Vector3 delta_Movement)
    {
        delta_Movement.z = 0;
        transform.Translate(delta_Movement, Space.World);
    }

    /// <summary>
    /// Calculates the player's velocity vector for the next frame.
    /// </summary>
    /// <param name="delta_Movement"></param>
    /// <param name="current_Velocity">This is the velocity the player has during this frame.</param>
    /// <returns></returns>
    private Vector3 CalculateVelocity(Vector3 delta_Movement, Vector3 current_Velocity)
    {
        // only calculate velocity if we have a non-zero deltaTime
        if (Time.deltaTime > 0f)
        {
            return delta_Movement / Time.deltaTime;
        }
        else
        {
            return current_Velocity;
        }
    }
}
