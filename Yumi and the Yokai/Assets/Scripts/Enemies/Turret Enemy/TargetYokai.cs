﻿/*
 * Author: Keiran Glynn & Karim Dabboussi
 * Date Created: 3/17/2018 @ 11:30 am
 * Date Modified: 3/17/2018 @ 11:30 am
 * Project: CompSciClubFall2017
 * File: TargetYokai.cs
 * Description: This script provides a way for the turret enemy in the tutorial level to detect the player. If the astral component is close enough to
 * the enemy, this script will enable the enemy's other scripts, allowing it to shoot. When the Yokai enters the box collider of the turret enemy, 
 * this script will set TurretEnemy.inRange to a value of true.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetYokai : MonoBehaviour {
    //public static Transform targeter;
    //private void OnTriggerStay(Collider collision)
    //{
    //    if (collision.tag == "Human") 
    //    {
    //        targeter = GameObject.Find("Player-Human").transform;
    //        GameObject.Find("ProjectileSpawn").GetComponent<TurretEnemy>().SetInRange(true);
    //        Debug.Log("In range of enemy turret");
    //    } else if (collision.tag == "Yokai")
    //    {
    //        targeter = GameObject.Find("Yokai-Human").transform;
    //        GameObject.Find("ProjectileSpawn").GetComponent<TurretEnemy>().SetInRange(true);
    //        Debug.Log("In range of enemy turret");
    //    }
    //} // Activated when the astral is inside of the turret's range

    //private void OnTriggerExit(Collider collision)
    //{
    //    if (collision.tag == "Ferrox" || collision.tag == "Human")
    //    {
    //        GameObject.Find("ProjectileSpawn").GetComponent<TurretEnemy>().SetInRange(false);
    //        targeter = null;
    //        Debug.Log("Out of range of enemy turret");

    //    }
    //} // Resets the value of TurretEnemy.inRange to false when the astral goes out of range (leaves box collider)
}
