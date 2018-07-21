using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlapPlayerDetector : MonoBehaviour {
    bool canAttack = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canAttack = GameObject.Find("FlapEnemy").GetComponent<FlapMovement>().GetCanAttack();
        if (collision.tag == "Human" && canAttack)
        {
            GameObject.Find("FlapEnemy").GetComponent<FlapMovement>().SetOverPlayer(true);
            Debug.Log("Player under attack by flap!");
            GameObject.Find("FlapEnemy").GetComponent<FlapMovement>().SetOverPlayer(false);            
        }
    }
}
