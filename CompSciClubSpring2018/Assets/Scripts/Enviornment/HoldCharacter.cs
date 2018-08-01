/*
 * 
 * Programmer: Brenden Plong
 * Date Created: 7/25/2018
 * Date Updated: 8/1/2018
 * Description: Script will make it so that the player will stick onto the platform
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldCharacter : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        col.transform.parent = gameObject.transform;
    }
    void OnTriggerExit(Collider col)
    {
        col.transform.parent = null;
    }
}
