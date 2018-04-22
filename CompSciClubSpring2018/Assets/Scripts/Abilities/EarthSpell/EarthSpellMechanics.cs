/* EarthSpellMechanics.cs
 * Date Created: 3/15/18
 * Last Edited: 4/20/18
 * Programmer: Daniel Jaffe & Darrell Wong
 * Description: Functionality of Earth Spell - Attach to the earthSpell object (cube):
 *      1. When an earth spell is created from EarthSpellUse.cs, the properties of the earth spell are defined here
 *      2. Click and drag functionality decides growth direction
 *      3. Properties of growth speed/distance, maxSpells, and decay time are public variables
 *      4. Earth spell growth will stop on collision with specified objects with:
 *          a)"Floor" layer
 *          b)"Earth" layer
 *          c)"Earth Spell Object" tag
 *          
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EarthSpellMechanics : MonoBehaviour
{
    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private float deltaX = 0f, deltaY = 0f;
    private bool grow = false;
    private Collision col;
    private IEnumerator co;

    public int maxSpells = 3;
    public float extendFactor = 25f;
    public float extendSpeed = .1f;
    public float maxGrow = 3.0f;
    public int destroyTime = 5;
    public bool stopOnHitEarth = true;
    bool firstCollision;

    public bool tester; 

    //Start
    private void Start()
    {
        firstCollision = false;
        firstPressPos = Input.mousePosition;
        co = Grow(extendSpeed);
        StartCoroutine(co);
        Destroy(gameObject, destroyTime); //Destroy timer starts on creation
    }

    private void Update()
    {

        if (Input.GetMouseButtonUp(0))
        {
            secondPressPos = Input.mousePosition;
            
            //calculate the difference between firstposition and second position
            deltaX = secondPressPos.x - firstPressPos.x;
            deltaY = secondPressPos.y - firstPressPos.y;

            //print("First: " + firstPressPos.x + " " + firstPressPos.y + "\n");
            //print("Second: " + secondPressPos.x + " " + secondPressPos.y + "\n");
            grow = true;
        }

        if (GameObject.FindGameObjectsWithTag("Earth Spell Object").Length > maxSpells) //destroy the earliest spell when there are too many
        {
            Destroy(GameObject.FindGameObjectsWithTag("Earth Spell Object")[0]);
        }
    }

    //Called at start and is a slow progression loop
    private IEnumerator Grow(float extendSpeed)
    {
        //Waits until IsGrowing returns true;
        yield return new WaitUntil(IsGrowing);

        if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
        {
            if (deltaX > 0) {
                for (int i = 0; i < extendFactor; i++)
                {
                    //stretches object in the X direction
                    gameObject.transform.localScale += new Vector3(extendSpeed, 0, 0);
                    //moves the object along to accomidate for equal stretching on both sides
                    gameObject.transform.Translate((extendSpeed) / 2, 0, 0);
                    yield return new WaitForSeconds(extendSpeed / 10);
                }
            }
            if (deltaX < 0)
            {
                for (int i = 0; i < extendFactor; i++)
                {
                    //stretches object in the X direction
                    gameObject.transform.localScale += new Vector3(extendSpeed, 0, 0);
                    //moves the object along to accomidate for equal stretching on both sides
                    gameObject.transform.Translate((-extendSpeed) / 2, 0, 0);
                    yield return new WaitForSeconds(extendSpeed / 10);
                }
            }
        }
        else if (Mathf.Abs(deltaX) < Mathf.Abs(deltaY))
        {
            if (deltaY > 0)
            {
                for (int i = 0; i < extendFactor; i++)
                {
                    //stretches object in the Y direction
                    gameObject.transform.localScale += new Vector3(0, extendSpeed, 0);
                    //moves the object along to accomidate for equal stretching on both sides
                    gameObject.transform.Translate(0, (extendSpeed / 2), 0);
                    yield return new WaitForSeconds(extendSpeed / 10);
                }
            }
            if (deltaY < 0)
            {
                for (int i = 0; i < extendFactor; i++)
                {
                    //stretches object in the Y direction
                    gameObject.transform.localScale += new Vector3(0, extendSpeed, 0);
                    //moves the object along to accomidate for equal stretching on both sides
                    gameObject.transform.Translate(0, (-extendSpeed / 2), 0);
                    yield return new WaitForSeconds(extendSpeed / 10);
                }
            }
        }
    }

    //returns true or false based on if the object should be growing or not
    public bool IsGrowing()
    {
        if (grow)
        {
            return true;
        }
        else return false;
    }

    private void OnCollisionEnter(Collision col)
    {
        //The earth spell growth can be stopped by adding more tags to this if statement
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Floor")     
            || col.collider.gameObject.layer == LayerMask.NameToLayer("Earth")  
            || col.collider.gameObject.CompareTag("Earth Spell Object"))    
        {
            
            if (!firstCollision)    //This is used to negate the initial collision of instantiating the earth spell inside of an earth block
                                    //bool firstCollision is instantiated in the Start() function
            {
                firstCollision = true;
            }
            else
            { 
                //print("Stop Cooroutine");
                StopCoroutine(co);
            }
            
        }
    }

}
