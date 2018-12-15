using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour {

    public static bool StillOnWall(PlayerController playerController, bool slidingRight, BoxCollider boxCollider, bool isGrounded, LayerMask platformMask)
    {
        Vector3 ray = new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.center.y, 0f);
        float rayLength = boxCollider.bounds.extents.x + 0.01f;
        Vector3 rayDirection = slidingRight ? Vector3.right : Vector3.left;

        RaycastHit hit;

        Debug.DrawRay(ray, rayDirection * rayLength, Color.black);

        bool raycastHit = Physics.Raycast(ray, rayDirection, out hit, rayLength, platformMask);

        if (raycastHit)
        {
            if ((hit.transform.gameObject.tag == "WallJump") && !isGrounded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
