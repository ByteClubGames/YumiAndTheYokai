using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public InputEnabler inputEnabler;
        

    [Header("Collision Detection")]
    [Range(2, 5)]
    public int horizontalRays = 4;
    [Range(2, 5)]
    public int verticalRays = 3;
    public float rayLength = .1f;
    public LayerMask platformMask;
    public float angleLimit = 45;

    private float verticalRaySeparation;
    private float horizontalRaySeparation;

    #region Box Collider Bounds
    private BoxCollider boxCollider;
    private Vector3 TL; // Top Left corner of the box collider
    private Vector3 TR; // Top Right corner of the box collider
    private Vector3 BL; // Bottom Left corner of the box collider
    private Vector3 BR; // Bottom Right corner of the box collider

    private void RaycastStartPoints()
    {
        TL = new Vector3(boxCollider.bounds.min.x, boxCollider.bounds.max.y, 0f);
        TR = new Vector3(boxCollider.bounds.max.x, boxCollider.bounds.max.y, 0f);
        BL = new Vector3(boxCollider.bounds.min.x, boxCollider.bounds.min.y, 0f);
        BR = new Vector3(boxCollider.bounds.max.x, boxCollider.bounds.min.y, 0f);
    }
    #endregion

    
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }



    private void Awake()
    {
        //set the ie
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        verticalRaySeparation = boxCollider.size.y / (horizontalRays - 1);
        horizontalRaySeparation = boxCollider.size.x / (verticalRays - 1);

        CastRaysBottom();
        CastRaysTop();
        CastRaysLeft();
        CastRaysRight();
        
        
    }

    private void CastRaysRight()
    {
        var charstatus = inputEnabler.activeCharacterStatus;
        int halfNumberOfRays = horizontalRays / 2; // used to check for slopes on only some of the raycasts.

        for (int i = 0; i < horizontalRays; i++)
        {
            Vector3 ray = new Vector3(BR.x, BR.y + i * verticalRaySeparation, 0f);
            RaycastHit hit;
            Debug.DrawRay(ray, Vector3.right * rayLength, Color.magenta);
            bool raycastHit = Physics.Raycast(ray, Vector3.right, out hit, rayLength, platformMask);

            if (!raycastHit) { continue; } // Don't read anymore code if no RayCastHit occured.

            if(i < halfNumberOfRays) { checkSlopes(hit, true); }

            checkWalls(hit, true);

        }
    }

    private void CastRaysLeft()
    {
        var charstatus = inputEnabler.activeCharacterStatus;
        int halfNumberOfRays = horizontalRays / 2; // used to check for slopes on only some of the raycasts.

        for (int i = 0; i < horizontalRays; i++)
        {
            Vector3 ray = new Vector3(BL.x, BL.y + i * verticalRaySeparation, 0f);
            RaycastHit hit;
            Debug.DrawRay(ray, Vector3.left * rayLength, Color.magenta);
            bool raycastHit = Physics.Raycast(ray, Vector3.left, out hit, rayLength, platformMask);

            if (!raycastHit) { continue; } // Don't read anymore code if no RayCastHit occured.

            if (i < halfNumberOfRays) { checkSlopes(hit, false); }

            checkWalls(hit, false);

        }
    }

    private void CastRaysBottom()
    {
        var charstatus = inputEnabler.activeCharacterStatus;
        int halfNumberOfRays = verticalRays / 2; // used to check for slopes on only some of the raycasts.

        for (int i = 0; i < verticalRays; i++)
        {
            Vector3 ray = new Vector3(BL.x + i * horizontalRaySeparation, BL.y, 0f);
            RaycastHit hit;
            Debug.DrawRay(ray, Vector3.down * rayLength, Color.magenta);
            bool raycastHit = Physics.Raycast(ray, Vector3.down, out hit, rayLength, platformMask);

            if (!raycastHit) { continue; } // Don't read anymore code if no RayCastHit occured.

            /* Check for slopes beneath the player */
            if(i < halfNumberOfRays)
            {
                checkSlopes(hit, false);
            }
            else
            {
                checkSlopes(hit, true);
            }

            charstatus.setOnGround(true);
        }
    }

    private void CastRaysTop()
    {
        var charstatus = inputEnabler.activeCharacterStatus;

        for (int i = 0; i < verticalRays; i++)
        {
            Vector3 ray = new Vector3(TL.x + i * horizontalRaySeparation, TL.y, 0f);
            RaycastHit hit;
            Debug.DrawRay(ray, Vector3.up * rayLength, Color.magenta);
            bool raycastHit = Physics.Raycast(ray, Vector3.up, out hit, rayLength, platformMask);

            if (!raycastHit) { continue; } // Don't read anymore code if no RayCastHit occured.

            charstatus.setOnLowRoof(true);
        }
    }

    /// <summary>
    /// Will flag if a slope is present, and if it is too steep to climb.
    /// </summary>
    /// <param name="hit">The associated raycastHit.</param>
    /// <param name="isRightSlope">Enter true if there you are checking slopes on the right hand side. Otherwise, false.</param>
    private void checkSlopes(RaycastHit hit, bool isRightSlope)
    {
        float slopeAngle = Mathf.Atan2(hit.normal.x, hit.normal.y) * Mathf.Rad2Deg;

        if ((slopeAngle != 0) | (slopeAngle != 90)) // Does a non-perpendicular angle exist?
        {
            if (slopeAngle > angleLimit) // Is the angle too steep to stand on?
            {                
                if (isRightSlope)
                {
                    inputEnabler.activeCharacterStatus.setOnRightSlope_Steep(true);
                    inputEnabler.activeCharacterStatus.setOnRightWall(true);
                }
                else
                {
                    inputEnabler.activeCharacterStatus.setOnLeftSlope_Steep(true);
                    inputEnabler.activeCharacterStatus.setOnLeftWall(true);
                }
            }
            else // The angle isn't too steep, its just a regular slope.
            {
                if (isRightSlope)
                {
                    inputEnabler.activeCharacterStatus.setOnRightSlope(true);
                }
                else
                {
                    inputEnabler.activeCharacterStatus.setOnLeftSlope(true);
                }
            }
        }

        
    }

    private void checkWalls(RaycastHit hit, bool isRightSlope)
    {
        float slopeAngle = Mathf.Atan2(hit.normal.x, hit.normal.y) * Mathf.Rad2Deg;

        if ((slopeAngle == 0) | (slopeAngle == 90))
        {
            if (isRightSlope)
            {
                inputEnabler.activeCharacterStatus.setOnRightWall(true);
            }
            else
            {
                inputEnabler.activeCharacterStatus.setOnLeftWall(true);
            }
        }
    }


    //public GetCharacterStatus()
    //{

    //}
}
