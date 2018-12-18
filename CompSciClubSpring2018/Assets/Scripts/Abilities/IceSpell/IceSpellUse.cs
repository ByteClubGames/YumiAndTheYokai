/* 
********************************************************************************
*Creator(s)............................................Jack Bruce, Brenden Plong 
*Created..............................................................03/10/2018 
*Last Modified............................................@ 5:00PM on 12/14/2018 
*Last Modified by................................................Michael Sanchez 
* 
*Description:   This script is used to instantiating ice spell objects upon 
*               mouse clicks. Update @ 9/13/2018: Changed previous method of 
*               casting the spell objects to ray casting to allow for more 
*               "accurate" spawning. 
********************************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class IceSpellUse : MonoBehaviour
{
    public GameObject IceProjectile;
    //private Plane referencePlane = new Plane(new Vector3(0, 0, -1), new Vector3(0, 0, 0));

    //public Vector3 GetSpellClickTarget()
    //{
    //    Vector3 mousePosFar = new Vector3(Input.mousePosition.x,
    //                                      Input.mousePosition.y,
    //                                      Camera.main.farClipPlane);
    //    Vector3 mousePosNear = new Vector3(Input.mousePosition.x,
    //                                       Input.mousePosition.y,
    //                                       Camera.main.nearClipPlane);
    //    Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
    //    Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

    //    // shoot a ray from nearClipPlane -> farClipPlane 
    //    Ray screenPoint = new Ray(mousePosN, mousePosF - mousePosN);

    //    // get worldCursorPosition/vector3 of spell click target on reference plane
    //    float distanceCamToPlane = 0f;
    //    referencePlane.Raycast(screenPoint, out distanceCamToPlane);
    //    Vector3 worldCursorPosition = screenPoint.GetPoint(distanceCamToPlane);

    //    // final spell path direction = projected vector from (mousePosF - mousePosN) onto reference plane 
    //    worldCursorPosition = Vector3.ProjectOnPlane(worldCursorPosition, new Vector3(0, 0, 0));

    //    // get SpellCaster current position
    //    Vector3 castPos = GameObject.Find("SpellCaster").transform.position;

    //    // unity debug rays. blue: yumi -> spell click point, red: yumi -> (mousePosF - mousePosN) 
    //    UnityEngine.Debug.DrawRay(castPos, worldCursorPosition - castPos, Color.blue);
    //    UnityEngine.Debug.DrawRay(mousePosN, (mousePosF - mousePosN) * 100, Color.red);

    //    // fix z-coordinate
    //    Vector3 direction = worldCursorPosition - castPos;
    //    direction.z = 0;

    //    // final spellClickTarget is (world position - yumi's position)
    //    return direction;
    //}

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("instantiate");
            Vector3 spawnLoc = GameObject.Find("SpellCaster").transform.position;
            //spawnLoc.z = 0;

            //GameObject clone = Instantiate(IceProjectile, spawnLoc, Quaternion.FromToRotation(Vector3.up, GetSpellClickTarget().normalized)) as GameObject;
            GameObject clone = Instantiate(IceProjectile, spawnLoc, Quaternion.identity) as GameObject;
        }
    } // end Update(): void

} // end class IceSpellUse : Monobehaviour