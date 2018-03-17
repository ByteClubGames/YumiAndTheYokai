/*
 *  
 * Author: Spencer Wilson
 * Date Created: 3/16/2018 @ 8:38 pm
 * Date Modified: 3/6/2018 @ 8:38 pm
 * Project: CompSciClubSpring2018
 * File: CircuitPuzzle.cs
 * Description: Script that controls the circuit puzzle.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitPuzzleController : MonoBehaviour {

    public GameObject switch1;
    public GameObject switch2;
    public GameObject switch3;
    public GameObject switch4;

    public GameObject ring1; // Holds the reference to the Ring1 game object.
    public GameObject ring2; // Holds the reference to the Ring2 game object.
    public GameObject ring3; // Holds the reference to the Ring3 game object.
    public GameObject ring4; // Holds the reference to the Ring4 game object.

    public void Update()
    {
        TestRings();
    }

    public void TestRings()
    {
        if(Input.GetKey("1"))
        {
            RotateRing1();
        }
        if(Input.GetKey("2"))
        {
            RotateRing2();
        }
        if(Input.GetKey("3"))
        {
            RotateRing3();
        }
        if(Input.GetKey("4"))
        {
            RotateRing4();
        }
    }

    public void RotateRing1() // Rotates Ring1, the center ring.
    {
        //StartCoroutine(Rotate(ring1, 9f));
        //RotateRing(ring1, new Vector3(0f, 0f, 1f) * 90f, 10f);
        ring1.GetComponent<Transform>().Rotate(new Vector3(0, 0, 1) * 315 * Time.deltaTime);
    }

    public void RotateRing2() // Rotates Ring2, the ring second from the center.
    {
        //RotateRing(ring2, new Vector3(0f, 0f, 1f) * 90f, 10000f);
        ring2.GetComponent<Transform>().Rotate(new Vector3(0, 0, -1) * 90 * Time.deltaTime);
    }

    public void RotateRing3() // Rotates Ring3, the ring third from the center.
    {
        //RotateRing(ring3, new Vector3(0f, 0f, 1f) * 90f, 0.9f);
        ring3.GetComponent<Transform>().Rotate(new Vector3(0, 0, 1) * 90 * Time.deltaTime);
    }

    public void RotateRing4() // Rotates Ring4, the outermost ring.
    {
        //RotateRing(ring4, new Vector3(0f, 0f, 1f) * 90f, 0.1f);
        ring4.GetComponent<Transform>().Rotate(new Vector3(0, 0, -1) * 90 * Time.deltaTime);
    }

    IEnumerator Rotate(GameObject ring, float time)
    {
        yield return null;
    }
}
