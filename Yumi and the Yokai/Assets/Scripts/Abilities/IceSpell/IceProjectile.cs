/*
********************************************************************************
*Creator(s)......................................................Michael Sanchez
*Created..............................................................12/15/2018
*Last Modified...........................................@ 10:20PM on 12/21/2018
*Last Modified by................................................Michael Sanchez
*
*Description:   Handles behavior for the Ice Projectile instantiated by the
*               IceSpellUse.cs script. Instantiates iceSpellPrefab upon
*               colliding with GameObject.
********************************************************************************
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    public float freq;
    public float speed;
    public float duration;
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

        // start movement coroutine, destroys object upon completion
        coroutine = ProjectileMovement(spellClickTarget);
        StartCoroutine(coroutine);

        // extra precaution, destroy projectile after 'duration' seconds
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        if (transform.position == spellClickTarget) {
            Destroy(gameObject);
        }
    }

    private IEnumerator ProjectileMovement(Vector3 target)
    {
        print("coroutine");
        print(spellClickTarget.magnitude);
        //while(Vector3.Distance(transform.position, spellClickTarget) > 10f)
        for(double i=0; i < spellClickTarget.magnitude; i+=speed)
        {
            // translate along object's y-axis. z: sin oscillation, x: cos oscillation
            transform.Translate(speed * Vector3.up, Space.Self);
            transform.Translate(speed * (Vector3.forward * Mathf.Sin(Time.time*freq))/2, Space.Self);
            transform.Translate(speed * (Vector3.right * Mathf.Cos(Time.time*freq))/2, Space.Self);

            // instantiate ice particles at current projectile location
            Instantiate(iceSpellPrefab, transform.position, Quaternion.identity);
            yield return null;
        }
        // destroy projectile once spellClickTarget.magnitude is reached
        Destroy(gameObject);

    }

    private void OnCollisionEnter(Collision collision)
    {
        print("collision detected!");
        if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            // spawn ice spell particle effect at collision location
            Instantiate(iceSpellPrefab, collision.contacts[0].point, Quaternion.identity);
            Destroy(gameObject); // end spell life-cycle
        }
    }

    public Vector3 GetSpellClickTarget()
    {
        // take mouse input as Vector3 coord. in the far and near plane
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
