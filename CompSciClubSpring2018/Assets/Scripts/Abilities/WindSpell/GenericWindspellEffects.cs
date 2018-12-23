/*GenericWindspellEffects.cs
********************************************************************************
*Creator(s).........................................................Darrell Wong
*Created................................................................12/14/18
*Last Modified..........................................................12/21/18
*Last Modified by...................................................Darrell Wong
*
*   Description: To be attached to enemies or pushable objects. 
*   
*                   1. applies a force when a windspell hit the object this is attached to
*
********************************************************************************
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericWindspellEffects : MonoBehaviour {

    private Rigidbody rb;
    private Rigidbody actualWindSpell;
    private Rigidbody windSpell;

    public float force = 10f;
    private bool pushingRight;
    private bool pushing;
    private Vector3 velocity;
    private float velocity2;
	// Use this for initialization
	void Start () {
        rb = this.gameObject.GetComponent<Rigidbody>();
        actualWindSpell = GameObject.FindGameObjectWithTag("WindSpellAgent").GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (pushing)
        {
            if (pushingRight)
            {
                CollisionWithObject(gameObject, Vector3.right);
            }
            else
            {
                CollisionWithObject(gameObject, Vector3.left);
            }
        }
        //velocity = actualWindSpell.velocity;
        //if (actualWindSpell.velocity.x > 0)
        //{
        //    pushingRight = true;
        //}
        //else
        //{
        //    pushingRight = false;
        //}
        //velocity = windSpell.velocity;
	}

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "WindSpellAgent") //need to differentiate between normal windspell(drawn) or agent(AI navmesh) because accessing velocies are different
        {
            //print("windspellAgent hit");
            WindSpellAgent windspellAgent = col.GetComponent<WindSpellAgent>();
            velocity = windspellAgent.velocity;
            //print(windspellAgent.velocity.x);
            if (windspellAgent.velocity.x >= 0)
            {
                pushingRight = true;
            }
            else
            {
                pushingRight = false;
            }
            pushing = true;
        }
        if (col.tag == "WindSpell")
        {
            //print("windspell hit");
            WindSpellMover windSpell = col.GetComponent<WindSpellMover>();
            velocity2 = windSpell.velocity;
            if (windSpell.velocity >= 0)
            {
                pushingRight = true;
            }
            else
            {
                pushingRight = false;
            }
            pushing = true;
        }
    }

    //private void CollisionWithEnemy(GameObject enemy)
    //{
    //    if (enemy is kinematic)
    //    {
    //        //add force
    //    }

    //    else if (enemy is !kinematic)
    //    {

    //    }
    //}

    private void CollisionWithObject(GameObject movedObject, Vector3 forceDirection)
    {
        if(!rb.isKinematic)
        {
            rb.AddForce(forceDirection * force);
            pushing = false;
        }
    }
}
