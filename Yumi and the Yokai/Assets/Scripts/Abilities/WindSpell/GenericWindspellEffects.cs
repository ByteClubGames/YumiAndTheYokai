/*GenericWindspellEffects.cs
********************************************************************************
*Creator(s).........................................................Darrell Wong
*Created................................................................12/14/18
*Last Modified..........................................................2/9/2019
*Last Modified by...................................................Darrell Wong
*
*   Description: To be attached to enemies or pushable objects. 
*   
*                   1. applies a coroutine moveposition when a windspell hit the object this is attached to
*                   2. Pushes object in the direction of the windspell
*                   
*                   NOTE: disables all scripts to allow moveposition to work. 
*                         I could not figure out how to make a generic way to 
*                         disable movement scripts so I disabled them all. If disabling
*                         the scripts causes problems, then using
*                         
*                         varGameObject.GetComponent<scriptname>().enabled = true;
*                         
*                         or something similar can be used to replace disable/enableScripts().
*                         This would need to be done on each enemy individually.
*
********************************************************************************
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericWindspellEffects : MonoBehaviour
{

    private Rigidbody rb;
    // private Rigidbody actualWindSpell;
    private Rigidbody windSpell;

    //public float force = 10f;
    public float pushTime = 5;
    public float pushSpeed = 10;
    private bool pushingRight;
    private bool pushing;
    private bool movementDisabled = false;
    private Vector3 velocity;
    private Vector3 velocity2;
    private Vector3 finalPushDirection;
    // Use this for initialization

    private IEnumerator coroutine;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        //actualWindSpell = GameObject.FindGameObjectWithTag("WindSpellAgent").GetComponent<Rigidbody>();

        //coroutine = Push();

    }


    void Update()
    {
        if (pushing)
        {
            disableScripts();
            StartCoroutine(Push());
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "WindSpellAgent") //need to differentiate between normal windspell(drawn) or agent(AI navmesh) because accessing velocies are different
        {
            //print("windspellAgent hit");
            WindSpellAgent windspellAgent = col.GetComponent<WindSpellAgent>();
            velocity = windspellAgent.velocity;
            //print(windspellAgent.velocity.x);
            if (windspellAgent.velocity.x > 1 || windspellAgent.velocity.x < 1)
            {
                //pushingRight = true;
                finalPushDirection = new Vector3(velocity.x, velocity.y, 0).normalized;
                pushing = true;
            }
            //else if (windspellAgent.velocity.x < 1)
            //{
            //    pushingRight = false;
            //    pushing = true;
            //}
            else
            {
                pushing = false;
            }
        }
        if (col.tag == "WindSpell")
        {
            velocity2 = col.GetComponent<WindSpellMover>().getVelocity();
            //print("object windspell veloc: " + velocity2);
            if (velocity2.x > 1 || velocity2.y > 1 || velocity2.x < 1 || velocity2.y < 1)
            {
                finalPushDirection = new Vector3(velocity2.x, velocity2.y, 0).normalized;
                pushing = true;
            }
            //else if (velocity2.x < 1 || velocity2.y < 1)
            //{
            //    pushingRight = false;
            //    pushing = true;
            //}
            else
            {
                pushing = false;
            }
        }
    }

    IEnumerator Push()
    {
        movementDisabled = true;
        disableScripts();
        for (int i = 0; i < pushTime; i++)
        {

                rb.MovePosition(transform.position + finalPushDirection * pushSpeed * Time.deltaTime);


            yield return new WaitForSeconds(.01f);
        }
        pushing = false;
        movementDisabled = false;
        enableScripts();
        //print("push with coroutine.");
    }

    public void disableScripts()
    {
        MonoBehaviour[] scripts = gameObject.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
    }

    public void enableScripts()
    {
        MonoBehaviour[] scripts = gameObject.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = true;
        }
    }
}
