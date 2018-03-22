/*
 * Author: Keiran Glynn
 * Date Created: 3/17/2018 @ 11:30 am
 * Date Modified: 3/17/2018 @ 11:30 am
 * Project: CompSciClubFall2017
 * File: TurretProjectile.cs
 * Description:
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetYokai : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ferrox")
        {
            GameObject.Find("ProjectileSpawn").GetComponent<TurretEnemy>().SetInRange(true);
            Debug.Log("In range of enemy turret");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ferrox")
        {
            GameObject.Find("ProjectileSpawn").GetComponent<TurretEnemy>().SetInRange(false);
            Debug.Log("Out of range of enemy turret");
        }
    }
}
