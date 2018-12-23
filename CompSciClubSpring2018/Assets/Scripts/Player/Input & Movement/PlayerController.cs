/*
********************************************************************************
*Creator(s)...........................................Kieran Glynn, Darrell Wong
*Created...............................................................7/26/2018
*Last Modified...........................................@ 10:00PM on 12/20/2018
*Last Modified by...................................................Darrell Wong
*
*Description:   This script is the definition of the players physics. It handles:
*               1. Movement (Jump and Strafe)
*               2. Collisions using raycasts
*           
*           Notes:  Jump buffering technique (coyote time) inspired by Evan's work in the legacy
*                   HumanJump.cs
********************************************************************************
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour {

    public enum CharacterName {
        Yokai,
        Yumi
    }

    public CharacterName characterName;

    [Header("Physics/ Player Attributes")]
    public float gravity = -25f; // Negative input value
    public float jumpSpeed = 3f;
    public float headCheck = 1.1f;      // disables jumping when under a low ceiling, look for headCheckRaycast
    public float horizontalSpeed = 8f; // Movement speed along horizontal axis
    public float wallSlideSpeed = 2f;
    public float wallJumpXSpeed = 6f;
    public float wallJumpYSpeed = 8f;
    public float wallJumpXDistance = 1f;
    public float maxClimbableSlope = 50f; //in degrees
    public float skinWidth; // Acts as an inset start point on the collider for the Ray orgin points
    public float shortHopCoefficient = 2f;   //decides how snappy the shorthop will be

    [Header("Collision Detection")]
    [Range(2, 12)]
    public int horizontalRays = 8;
    [Range(2, 10)]
    public int verticalRays = 4;
    public float error = .01f;
    public LayerMask platformMask;

    #region Hit Detection Flags    
    private bool climbableSlope;
    private bool isGrounded = false;
    private bool isOnSlope = false;
    private bool isRight = false;

    public void SetClimbableSlope(bool flag)
    {
        climbableSlope = flag;
    }

    public void SetIsGrounded(bool flag)
    {
        isGrounded = flag;
    }

    public void SetIsOnSlope(bool flag)
    {
        isOnSlope = flag;
    }

    public void SetIsRight(bool flag)
    {
        isRight = flag;
    }
    private bool isOnWallRight = false;
    private bool isOnWallLeft = false;

    public void SetIsOnWallRight(bool flag)
    {
        isOnWallRight = flag;
    }

    public void SetIsOnWallLeft(bool flag)
    {
        isOnWallLeft = flag;
    }

    public void ClearOnWall()
    {
        isOnWallLeft = false;
        isOnWallRight = false;
    }
    #endregion

    #region Movement Call Flags
    private bool right = false;
    private bool left = false;
    private bool jump = false;
    private bool bufferedJump = false;
    private bool shortHop = false;
    bool wallJumpActive = false;
    bool wallJumpStarted = false;
    bool firstPassFlag = false;

    /* the purpose of forcedShortHop is to eliminate a bug where you can very quickly tap and release the jump button
     * during the airBuffer frames resulting in a full jump rather than a short hop
     *
     * it will be used to force a short hop when jump is release before touching the ground not held down
     */
    private bool forcedShortHop = false;

    private bool wallJump = false;
    #endregion

    #region Box Collider Bounds
    private BoxCollider boxCollider;
    private Vector3 TL; // Top Left corner of the box collider
    private Vector3 TR; // Top Right corner of the box collider
    private Vector3 BL; // Bottom Left corner of the box collider
    private Vector3 BR; // Bottom Right corner of the box collider

    private void RaycastStartPoints()
    {
        var skinBounds = boxCollider.bounds;
        skinBounds.Expand(-2f * skinWidth);

        TL = new Vector3(skinBounds.min.x, skinBounds.max.y, 0f);
        TR = new Vector3(skinBounds.max.x, skinBounds.max.y, 0f);
        BL = new Vector3(skinBounds.min.x, skinBounds.min.y, 0f);
        BR = new Vector3(skinBounds.max.x, skinBounds.min.y, 0f);
    }
    #endregion

    private float normalizedHorizontalSpeed = 0f;
    private float verticalRaySeparation;
    private float horizontalRaySeparation;
    private int airBufferFrames = -1;         // if falling, there is a buffer where you can press jump early before touching the ground and the jump will still register
    public int defaultAirBufferFrames = 5;
    private int groundBufferFrames = -1;      //if walking off a platform, there is a buffer where you can still jump
    public int defaultGroundBufferFrames = 5;

    private Vector3 velocity;
    Vector3 horizontalTarget = Vector3.zero;
    Vector3 jumpPos;
    Vector3 targetPos;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Transform yumi;
    private CinemachineVirtualCamera main_follow_camera;

    private void Awake()
    {
        boxCollider = this.GetComponent<BoxCollider>();
    }

    private void Start()
    {
        main_follow_camera = GameObject.Find("Main Follow Camera").GetComponent<CinemachineVirtualCamera>();
        yumi = GameObject.Find("Yumi").transform;

        animator = this.gameObject.GetComponentInChildren<Animator>();
        spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        if (characterName == CharacterName.Yumi) {
            
        }
        if (characterName == CharacterName.Yokai) {
            main_follow_camera.Follow = this.gameObject.transform;
        }
    }

    private void OnDestroy()
    {
        if (characterName == CharacterName.Yokai) {
            CinemachineVirtualCamera main_follow_camera = GameObject.Find("Main Follow Camera").GetComponent<CinemachineVirtualCamera>();
            Transform yumi = GameObject.Find("Yumi").transform;
            main_follow_camera.Follow = yumi;
        }
    }
    
    #region Movement Calls
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
        if (isGrounded)
        {
            jump = true;
        }
        else if (isOnWallLeft && (characterName == CharacterName.Yokai))
        {
            wallJump = true;
        }
        else if (isOnWallRight && (characterName == CharacterName.Yokai))
        {
            wallJump = true;
        }

        if (!isGrounded && groundBufferFrames > 0)
        {
            bufferedJump = true;
        }

        if (!isGrounded && airBufferFrames <= 0)
        {
            //only reset air buffer if falling and pressing (not holding down) the jump button
            //checking airBufferFrames prevents a player from mashing jump for the frame perfect jumps. jumps should be carefully timed
            airBufferFrames = defaultAirBufferFrames;
        }
    }

    public void CallShortHop()
    { 
        if (airBufferFrames > 0 && velocity.y < 0)  //when jump is released while still falling it will force a short hop. without this, it would result in a full jump
        {
            forcedShortHop = true;
        }
        shortHop = true;

    }

    public void ClearCalls()
    {
        right = left = false;
    }
    #endregion

    void Update()
    {
        if(wallJump || wallJumpActive)
        {
            wallJump = false;
            wallJumpActive = true;



            if (!firstPassFlag)
            {
                firstPassFlag = true; // Flag used to make sure this block is only executed once per wall jump
                jumpPos = this.transform.position;
                targetPos = this.transform.position;

                if (isOnWallRight)
                {
                    normalizedHorizontalSpeed = -1;
                    targetPos += new Vector3(-1f, 0f, 0f);
                }
                else if (isOnWallLeft)
                {
                    normalizedHorizontalSpeed = 1;
                    targetPos += new Vector3(1f, 0f, 0f);
                }

                velocity.y = Mathf.Sqrt(2f * wallJumpYSpeed * -gravity);
                velocity.x = Mathf.Lerp(velocity.x, normalizedHorizontalSpeed * wallJumpXSpeed, Time.deltaTime * 20f);
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }

            /* We want to continue our wall jump code, unless the player has wall jump a specific horizontal distance away from the wall.
             * If the player goes this distance away from the wall, we go back to using our normal movement code until the next wall jump */
            if ((Mathf.Abs(this.transform.position.x - targetPos.x) > wallJumpXDistance))
            {
                wallJumpActive = false;
            }

            Move(velocity * Time.deltaTime);
        }
        else
        {
            AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            firstPassFlag = false;

            if (airBufferFrames > 0) airBufferFrames--;
            if (groundBufferFrames > 0) groundBufferFrames--;


            /* If the player is grounded, set vertical velocity to zero. This stops the player from accelerating downward */
            if (isGrounded)
            {
                velocity.y = 0;
                shortHop = false;

                groundBufferFrames = defaultGroundBufferFrames;
            }

            if (isOnWallLeft)
            {
                //flip sprite|animation
                velocity.y = wallSlideSpeed * -1 * Time.deltaTime * 10.0f;
                isOnWallLeft = global::WallJump.StillOnWall(this, false, boxCollider, isGrounded, platformMask);

                //if (wallJump)
                //{
                //    wallJump = false;
                //    wallJumpActive = true;

                //    velocity.x = wallJumpXSpeed * Time.deltaTime * 100f;
                //}

            }
            else if (isOnWallRight)
            {
                //flip sprite|animation
                velocity.y = wallSlideSpeed * -1 * Time.deltaTime * 10.0f;
                isOnWallRight = global::WallJump.StillOnWall(this, true, boxCollider, isGrounded, platformMask);

                //if (wallJump)
                //{
                //    wallJump = false;
                //    wallJumpActive = true;

                //    velocity.x = wallJumpXSpeed * -1.0f * Time.deltaTime * 100f;
                //}

            }

            //if (wallJumpActive)
            //{
            //    velocity.y = Mathf.Sqrt(2f * wallJumpYSpeed * -gravity * 10f);
            //    wallJumpActive = false;
            //}

            /* The following selection decides the directioin of horizontal movement (right, left, none) */
            if (right && !isOnWallRight)
            {
                if (!currentStateInfo.IsName("yokai_run"))
                {
                    spriteRenderer.flipX = false;
                    animator.Play("yokai_run");
                }
                normalizedHorizontalSpeed = 1;

                isOnWallLeft = false;
            }
            else if (left && !isOnWallLeft)
            {
                if (!currentStateInfo.IsName("yokai_run"))
                {
                    spriteRenderer.flipX = true;
                    animator.Play("yokai_run");
                }
                normalizedHorizontalSpeed = -1;

                isOnWallRight = false;
            }
            else
            {
                normalizedHorizontalSpeed = 0;
            }

            //  HeadCheckRay fixes the clipping issue when jumping with very low ceiling

            Vector3 headCheckRay = new Vector3(((TR + TL) / 2).x, this.gameObject.transform.position.y, 0f);
            RaycastHit hit;

            bool headCheckRaycastHit = Physics.Raycast(headCheckRay, Vector3.up, out hit, headCheck, platformMask);
            print(headCheckRaycastHit);
            Debug.DrawRay(headCheckRay, Vector3.up * headCheck, Color.blue);


            /* This selection will make the player jump given:
             *      it is touching the ground platform and spacebar is pressed
             *      it is walking off a ledge and jump is pressed before groundbuffer frames are not zero. (bufferedJump)
             *      it is falling and airBufferFrames(activated by pressing jump) are not zero. (see CallJump() above) 
             */

            if ((isGrounded && jump || bufferedJump || isGrounded && airBufferFrames > 0) && !headCheckRaycastHit)
            {
                jump = false;
                bufferedJump = false;
                airBufferFrames = -1;
                groundBufferFrames = -1;

                isGrounded = false;
                velocity.y = Mathf.Sqrt(2f * jumpSpeed * -gravity);
            }

            if (!isGrounded && velocity.y > 0 && (shortHop || forcedShortHop))
            {
                shortHop = false;

                if (forcedShortHop)
                {
                    forcedShortHop = false;
                    velocity.y = velocity.y / (shortHopCoefficient * .7f);

                    /*If this looks stupid that is because it is:
                     * It seems that the input delay when the jump key is released 
                     * gives the character enough time travel before cutting the velocity in half
                     * 
                     * when doing a forcedshorthop though, there is no input delay and it happens instantly
                     * creating an unintended very very short hop. scaling the shortHopCoefficient by .7f simulates the same input delay
                     * so that it does not make a super short hop but a regular short hop.
                     */
                }

                else
                    velocity.y = velocity.y / shortHopCoefficient;
            }

            // resets jump when jumping under a low ceiling
            if (jump && headCheckRaycastHit)
            {
                jump = false;
                bufferedJump = false;
                airBufferFrames = -1;
                groundBufferFrames = -1;
            }

            if (characterName == CharacterName.Yokai)
            {
                if (!right && !left && !jump)
                {
                    animator.Play("yokai_idle");
                }
                if (jump)
                {
                    Debug.Log(jump);
                }
            }
            if (characterName == CharacterName.Yumi)
            {
                animator.Play("");
            }

            //Horizontal velocity
            velocity.x = Mathf.Lerp(velocity.x, normalizedHorizontalSpeed * horizontalSpeed, Time.deltaTime * 20f);

            // Gravity (Vertical component of Velocity if not jumping)
            velocity.y += gravity * Time.deltaTime;


            /*The velocities (speed and directions that we would like to move in) are multiplied by Time to turn them into a position that
             * we would like to move towards. The Move function then passes then modifies these positions based on if the player is colldiding
             * with the world. After the movement positions are modified to prevent passing through walls and floors, the player is moved using 
             * transform.translate. */
            Move(velocity * Time.deltaTime);
        }
    }

    public void Move(Vector3 deltaMovement)
    {
        /* Reset of collsion flags for detecting the ground */
        isGrounded = false;
        isOnSlope = false;

        /* Recalculates the corners of our collider box so that the detection rays can be casted accurately */
        RaycastStartPoints();


        
        if (deltaMovement.y < 0f)
        {
            deltaMovement = CollisionCorrections.VerticalSlopeDetection(this, deltaMovement, boxCollider, BL, BR, skinWidth,
                maxClimbableSlope, platformMask);
        }
            

        /* Check for collisions to the left and right, and modify the position we want to move towards if we aren't supposed to 
         * go there. */
        if (deltaMovement.x != 0f)
        {
            float transformHeight = transform.localScale.y;
            deltaMovement = CollisionCorrections.HorizontalCollision(this, deltaMovement, boxCollider, BL, BR, transformHeight, verticalRaySeparation,
                maxClimbableSlope, skinWidth, error, horizontalRays, climbableSlope, isRight, isGrounded, platformMask);

            if (characterName == CharacterName.Yumi)
            {
                ClearOnWall();
            }
        }


        /* Check for collisions with the ceilings and floors, and modify the position we want to move towards if we aren't supposed to 
         * go there. */
        if (deltaMovement.y != 0f)
        {
            float transformWidth = transform.localScale.x;
            deltaMovement = CollisionCorrections.VerticalCollision(this, deltaMovement, boxCollider, TL, BL, transformWidth, horizontalRaySeparation,
                skinWidth, error, verticalRays, isOnSlope, isRight, isGrounded, platformMask);
        }
            

        /* Given our 2.5D game, we should always be zero in the z-axis. Once all of the movement positions have been modified based on 
         * the players collisions, use transform.Translate to move the player towards those positions */
        deltaMovement.z = 0;
        transform.Translate(deltaMovement, Space.World);

        // only calculate velocity if we have a non-zero deltaTime
        if (Time.deltaTime > 0f)
            velocity = deltaMovement / Time.deltaTime;

        
        // to be used more in slope detection when its fixed
        if (isOnSlope)
            velocity.y = 0;
    }
}
