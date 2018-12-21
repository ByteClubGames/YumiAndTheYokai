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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("instantiate");
            Vector3 spawnLoc = GameObject.Find("SpellCaster").transform.position;

            GameObject clone = Instantiate(IceProjectile, spawnLoc, Quaternion.identity) as GameObject;
        }
    }

}