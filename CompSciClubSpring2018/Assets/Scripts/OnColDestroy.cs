/*
*
*Creator(s).........................................................Brenden Plong
*Created..............................................................10/05/2018
*Last Modified............................................@ 4:30PM on 10/19/2018
*Last Modified by...................................................Brenden Plong
*
*Description:   Modular script that is used to destroy objecs based on collisions
**
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnColDestroy : MonoBehaviour {
    public GameObject player; // Used for grabbing the "player" object
    public GameObject enemy; // Used for grabbing the "enemy" object
    public LayerMask foe;
    public LayerMask friend;
    //(other.gameObject.layer == 9) 
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Destroy(enemy);
            Debug.Log("Has Collided");
        }
    } 
}
