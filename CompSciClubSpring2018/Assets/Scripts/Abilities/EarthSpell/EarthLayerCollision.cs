/*
 *  EarthLayerCollision.cs
 *  Date Created: 4/20/18
 *  Last Updated: 4/20/18
 *  Programmer: Darrell Wong
 *  Purpose: to make the earth spell stop when it collides with another earth layer.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthLayerCollision : MonoBehaviour
{

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Earth Spell Object") //Same problem from before, Once the espell is created, it instantly stops growth
        {
            // Do something for box collider
            print("Stop coroutine in EarthLayerCollision.cs");
            col.gameObject.GetComponent<EarthSpellMechanics>().StopCoroutineGrow();
        }
    }
}
