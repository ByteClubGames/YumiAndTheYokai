/* 
********************************************************************************
*Creator(s)......................................................Michael Sanchez
*Created..............................................................12/15/2018 
*Last Modified............................................@ 8:40PM on 12/17/2018 
*Last Modified by................................................Michael Sanchez 
* 
*Description:   Handles behavior for the Ice Projectile instantiated by the
*               IceSpellUse.cs script. Instantiates iceSpellPrefab upon
*               colliding with GameObject.
*********************************************************************************
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    public int projectileSpeed;
    public GameObject iceSpellPrefab;
    private Plane referencePlane = new Plane(new Vector3(0, 0, -1), new Vector3(0, 0, 0));
    private Vector3 spellClickTarget;
    private IEnumerator coroutine;

    void Start()
    {
        print("object created");

        // get spellClickTarget and start ProjectileMovement coroutine
        spellClickTarget = GetSpellClickTarget();
        transform.rotation = Quaternion.FromToRotation(Vector3.up, spellClickTarget);

        coroutine = ProjectileMovement(spellClickTarget);
        StartCoroutine(coroutine);
    }

    private IEnumerator ProjectileMovement(Vector3 target)
    {
        print("coroutine");
        for(int i=0; i < projectileSpeed; i++)
        {
            transform.Translate(Vector3.up, Space.Self);

            Instantiate(iceSpellPrefab, transform.position, Quaternion.identity);
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("collision detected!");
        if (!collision.collider.CompareTag("Human"))
        {
            // spawn ice spell particle effect at collision location
            Instantiate(iceSpellPrefab, collision.contacts[0].point, Quaternion.identity);
            Destroy(gameObject); // end spell life-cycle
        }
    }

    public Vector3 GetSpellClickTarget()
    {
        Vector3 mousePosFar = new Vector3(Input.mousePosition.x,
                                          Input.mousePosition.y,
                                          Camera.main.farClipPlane);
        Vector3 mousePosNear = new Vector3(Input.mousePosition.x,
                                           Input.mousePosition.y,
                                           Camera.main.nearClipPlane);
        Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

        // shoot a ray from nearClipPlane -> farClipPlane 
        Ray screenPoint = new Ray(mousePosN, mousePosF - mousePosN);

        // get worldCursorPosition/vector3 of spell click target on reference plane
        float distanceCamToPlane = 0f;
        referencePlane.Raycast(screenPoint, out distanceCamToPlane);
        Vector3 worldCursorPosition = screenPoint.GetPoint(distanceCamToPlane);

        // final spell path direction = projected vector from (mousePosF - mousePosN) onto reference plane 
        worldCursorPosition = Vector3.ProjectOnPlane(worldCursorPosition, new Vector3(0, 0, 0));

        // get SpellCaster current position
        Vector3 castPos = GameObject.Find("SpellCaster").transform.position;

        // unity debug rays. blue: yumi -> spell click point, red: yumi -> (mousePosF - mousePosN) 
        UnityEngine.Debug.DrawRay(castPos, worldCursorPosition - castPos, Color.blue);
        UnityEngine.Debug.DrawRay(mousePosN, (mousePosF - mousePosN) * 100, Color.red);

        // fix z-coordinate
        Vector3 direction = worldCursorPosition - castPos;
        direction.z = 0;

        // final spellClickTarget is (world position - yumi's position)
        return direction;
    }
}
