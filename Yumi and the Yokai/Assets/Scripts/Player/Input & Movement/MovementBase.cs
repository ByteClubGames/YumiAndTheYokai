using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBase : MonoBehaviour
{
    
    [Header("Collision Detection")]
    [Range(2, 12)]
    public int horizontalRays = 8;
    [Range(2, 10)]
    public int verticalRays = 4;
    public float error = .01f;
    public LayerMask platformMask;

    private Vector3 velocity;

    //private Animator animator;
    //private SpriteRenderer spriteRenderer;

    //private Transform yumi;

    //private List<Vector3> movementDeltas;
    private Queue movementDeltas = new Queue();

    float friction = 0f;

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
        AddMovementDelta(new Vector3(deltaMovement.x - friction, deltaMovement.y));
        // deltaMovement = AdjustDeltaMovement();
        Move(deltaMovement);
        velocity = CalculateVelocity(deltaMovement, velocity);
    }

    /// <summary>
    /// Checks the location of where the player is supposed to move to see if the player will collide with any platforms.
    /// If collision does occur, it will subtract from the proposed movement vector so that it doesn't move inside of a collider. 
    /// </summary>
    /// <param name="delta_Movement">This is the proposed movement vector for the next frame.</param>
    /// <returns></returns>
    private Vector3 AdjustDeltaMovement(Vector3 delta_Movement)
    {

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
    /// Zeroes out the X-direction of the proposed movement deltas for the next frame. Only use this if you don't want the character
    /// to move horizontally.
    /// </summary>
    public void ClearMovement_X()
    {
        Vector3 delta_Movement = SumMovementDeltas(movementDeltas);
        delta_Movement.x = 0f;
        AddMovementDelta(delta_Movement);
    }

    /// <summary>
    /// Zeroes out the Y-direction of the proposed movement deltas for the next frame. Only use this if you don't want the character
    /// to move vertically.
    /// </summary>
    public void ClearMovement_Y()
    {
        Vector3 delta_Movement = SumMovementDeltas(movementDeltas);
        delta_Movement.y = 0f;
        AddMovementDelta(delta_Movement);
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
