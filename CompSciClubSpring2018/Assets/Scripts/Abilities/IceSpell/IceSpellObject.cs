using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    public GameObject iceSpellPrefab;
    private Plane referencePlane = new Plane(new Vector3(0, 0, -1), new Vector3(0, 0, 0));
    Rigidbody spellObjectRB;
    private bool isActive;

    void Start()
    {
        print("object created");
        spellObjectRB = GetComponent<Rigidbody>();
        //transform.position = GameObject.Find("SpellCaster").transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("collision detected!");
        if (!collision.collider.CompareTag("player"))
        {
            // reset spellObject location to spellCaster position
            transform.position = GameObject.Find("SpellCaster").transform.position;
            Instantiate(iceSpellPrefab, GetSpellClickTarget(), Quaternion.identity);
            Destroy(gameObject, 3f); // end spell life-cycle
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

    void Update()
    {
        //if (isActive)
        //{
        //    print("should be moving now");
        //    spellObjectRB.MovePosition(transform.position + GetSpellClickTarget().normalized * 7 * Mathf.Sin(Time.deltaTime));
        //}
        //if (Vector3.Distance(transform.position, GetSpellClickTarget()) < 0.1f)
        //{
        //    isActive = false;
        //}
    }
}
