/*
 * Programmer:   Keiran Glynn
 * Date Created: 03/09/2018 @  12:30 PM 
 * Last Updated: 03/10/2018 @  12:30 PM 
 * File Name:    DamageGiver.cs 
 * Description:  This script is to be used for testing the mechanics of the FerroxHealth.cs and HumanHealth.cs classes. When it detects a 
 * collision between the human, or the ferrox, and the enemy entity, it gives damage to the player. Right now, due to the current tag 
 * assigned to the ferrox and human entities, the damage is shared between both entities regardless of which one earned the damage.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGiver : MonoBehaviour {

    public GameObject humanGameObject; // Public game object that holds the little girl game object.
    public GameObject ferroxGameObject; // Public game object that holds the ferrox's game object.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /* Both the Ferrox and the Human are currently tagged as the "Player." This means that given this DamageGiver.cs script, until both
         * the player and the ferrox recieve their own Tag values, they will share damage taken, regardless of which entity actually took damage.
         */
        if (collision.gameObject.CompareTag("Human") || collision.gameObject.CompareTag("Ferrox"))
        {
            humanGameObject.GetComponent<HumanHealth>().TakeDamage(6);
            ferroxGameObject.GetComponent<FerroxHealth>().TakeDamage(1);
        }
    }
}
