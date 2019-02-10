using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YumiMovement : MonoBehaviour {
    /// <summary>
    /// Includes all declarations for animator, sprite renderer, and integers for storing the
    /// animator state name hashes.
    /// </summary>
    #region Animation State Declarations
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private int deathHash;
    private int fallHash;
    private int idleHash;
    private int jumpHash;
    private int landingHash;    
    private int runHash;
    private int takeDamageHash;
    private int turnHash;
    #endregion

    //Declaration for object that will adjust movement when colliding
    private CollisionCorrections collision_corrector;
    private Transform playerTransform;
    private BoxCollider playerBoxCollider;


    [Header("Physics/ Player Attributes")]
    public float gravity = -25f; // Negative input value
    public float fallBuffer = .2f; // Time in seconds before fall animation should play when falling 
    private int airBufferFrames = -1;  // if falling, there is a buffer where you can press jump early before touching the ground and the jump will still register
    public int defaultAirBufferFrames = 5;
    private int groundBufferFrames = -1;      //coyote time
    public int defaultGroundBufferFrames = 5;

    [Space(1)]
    public float jumpSpeed = 3f;
    private float normalizedHorizontalSpeed = 0f;
    public float horizontalSpeed = 8f; // Movement speed along horizontal axis       
    public float shortHopCoefficient = 2f; // "snappieness of short hop"
    [Space(2)]

    [Header("Collision Detection")]
    public LayerMask platformMask;
    [Range(2, 12)]
    public int horizontalRays = 8;
    [Range(2, 10)]
    public int verticalRays = 4;
    public float skinWidth; // Acts as an inset start point on the collider for the Ray orgin points
    public float headCheck = 1.1f;      // disables jumping when under a low ceiling, look for headCheckRaycast
    public float error = .01f;
    public float maxClimbableSlope = 50f; //in degrees 

    //Will be multiplied by time to become a movement vector
    private Vector3 velocity;

    bool right;
    bool left;
    bool jump;

    public void CallRight(bool call)
    {
        if (call)
        {
            right = true;
        }
        else
        {
            right = false;
        }
    }

    public void CallLeft(bool call)
    {
        if (call)
        {
            left = true;
        }
        else
        {
            left = false;
        }
    }

    public void CallJump()
    {
        jump = true;
    }

    //public void CallShortHop()
    //{
    //    if (airBufferFrames > 0 && velocity.y < 0)  //when jump is released while still falling it will force a short hop. without this, it would result in a full jump
    //    {
    //        forcedShortHop = true;
    //    }
    //    shortHop = true;

    //}




    // Use this for initialization
    void Start () {
        PrepareAnimatorAndAnimationStates();
        
    }

    /// <summary>
    /// Initializes the playerTransform and playerBoxCollider, then passes them into a constructor for the collsion corrector object.
    /// This object will handle checks for the player colliding with platforms. If collisions exist, it will adjust the
    /// player's movement so that it does not clip into the platform.
    /// </summary>
    void PrepareCollisionCorrector()
    {
        playerTransform = GetComponent<Transform>();
        playerBoxCollider = GetComponent<BoxCollider>();
        collision_corrector = new CollisionCorrections(playerTransform, playerBoxCollider, platformMask, verticalRays,
            horizontalRays, skinWidth, headCheck, maxClimbableSlope, error);
    }
	
	// Update is called once per frame
	void Update () {
		

	}

    private void FixedUpdate()
    {
        if (airBufferFrames > 0) airBufferFrames--;
        if (groundBufferFrames > 0) groundBufferFrames--;

        SetHorizontalDirection();

        ApplyHorizontalMovement();


        StateProcesses(anim.GetCurrentAnimatorStateInfo(0));



    }

    /// <summary>
    /// Sets the direction that the player should be moving on the horizontal axis, and flips the sprite to the
    /// given direction of movement.
    /// </summary>
    void SetHorizontalDirection()
    {
        if (right)
        {
            normalizedHorizontalSpeed = 1f;
            spriteRenderer.flipX = true;
        }
        else if (left)
        {
            normalizedHorizontalSpeed = -1f;
            spriteRenderer.flipX = false;
        }
        else
        {
            normalizedHorizontalSpeed = 0f;
        }
    }

    /// <summary>
    /// Adjusts your vertical velocity as if the force of gravity was acting downward against the object.
    /// </summary>
    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    void Move(float current_velocity)
    {

    }

    /// <summary>
    /// Gives the player a velocity in the direction of movement.
    /// </summary>
    void ApplyHorizontalMovement()
    {
        velocity.x = Mathf.Lerp(velocity.x, normalizedHorizontalSpeed * horizontalSpeed, Time.deltaTime * 20f);
    }

    /// <summary>
    /// Section of code for implementing state specific processes. 
    /// </summary>
    /// <param name="current_state_info">The current animtion state info.</param>
    void StateProcesses(AnimatorStateInfo current_state_info)
    {
        int deathState = deathHash;
        int fallState = fallHash;
        int idleState = idleHash;
        int jumpState = jumpHash;
        int landingState = landingHash;
        int runState = runHash;
        int takeDamageState = takeDamageHash;
        int turnState = turnHash;
        int currentStateHash = current_state_info.shortNameHash;

        if(currentStateHash == deathState)
        {
            velocity.y = 0;
        }
        else if(currentStateHash == fallHash)
        {

        }
        else if (currentStateHash == idleHash)
        {
            velocity.y = 0;
        }
        else if (currentStateHash == jumpHash)
        {

        }
        else if (currentStateHash == landingHash)
        {
            velocity.y = 0;
        }
        else if (currentStateHash == runHash)
        {

        }
        else if (currentStateHash == takeDamageHash)
        {

        }
        else if (currentStateHash == turnHash)
        {

        }
        else
        {
            //default case.
        }

    }

    /// <summary>
    /// Initializes the animator, sprite renderer, and calculates the animation state hashes. The animation state hashes allow you 
    /// to compare which animation state you are in via int comparison (fast) instead of string comparison (slow).
    /// </summary>
    private void PrepareAnimatorAndAnimationStates()
    {
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        calculateAnimationStateHash();
    }

    /// <summary>
    /// Calculates an integer hash for the name strings of each of the animation states. These integers allow for a 
    /// very fast comparison for which animation state you are in. 
    /// </summary>
    private void calculateAnimationStateHash()
    {
        deathHash = Animator.StringToHash("yumiDeath");
        fallHash = Animator.StringToHash("yumiFall");
        idleHash = Animator.StringToHash("yumiIdle");
        jumpHash = Animator.StringToHash("yumiJump");
        landingHash = Animator.StringToHash("yumiLanding");
        runHash = Animator.StringToHash("yumiRun");
        takeDamageHash = Animator.StringToHash("yumiTakeDamage");
        turnHash = Animator.StringToHash("yumiTurn");
    }
}
