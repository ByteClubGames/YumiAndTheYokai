/*
********************************************************************************
*Creator(s)........................................................Hunter Goodin
*Created..............................................................12/14/2018
*Last Modified...........................................@ 8:00 PM on 12/14/2018
*Last Modified by..................................................Hunter Goodin
*
*Description: When the player collides with an object with this script attached
*             to it, the player will take damage. 
********************************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public int damageDealtToYumi = 1;
    public int damageDealtToYokai = 20;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Yumi")
        {
            GameObject.Find("Yumi").GetComponent<YumiHealthSystem>().DamageDealer(damageDealtToYumi);
        }
        else if (col.gameObject.name == "Yokai(Clone)")
        {
            GameObject.Find("Yokai(Clone)").GetComponent<YokaiHealthSystem>().DamageYokai(damageDealtToYokai);
        }
    }
}
