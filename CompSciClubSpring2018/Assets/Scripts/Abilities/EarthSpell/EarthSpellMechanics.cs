/* EarthSpellMechanics.cs
 * Date Created: 3/15/18
 * Last Edited: 4/06/18
 * Programmer: Daniel Jaffe & Darrell Wong
 * Description: Functionality of Earth Spell - Attach to the earthSpell object (cube):
 *      1. Tests if there is collision with another gameobject. If so, despawns.
 *      2. Not yet implemented: Spawns in without mesh layer. If collision == false, then turn mesh layer on. 
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EarthSpellMechanics : MonoBehaviour {

    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private float deltaX = 0f, deltaY = 0f;

    public float extendFactor = 25f;
    public float extendSpeed = .1f;
    public float maxGrow = 3.0f;
    public int destroyTime = 5;
    private bool grow = false;

    public EarthSpellUse spell;
    
    //Start
    private void Start()
    {
        StartCoroutine(Grow(extendSpeed));
        Destroy(gameObject, destroyTime); //Destroy timer starts on creation
    }

    private void Update()
    {
        //get user input
        if (Input.GetMouseButtonDown(0))
        {
            //get first mouse position
            firstPressPos = spell.playerinput;
        }
        if (Input.GetMouseButtonUp(0))
        {
            //get second mouse position
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //calculate the difference between firstposition and second position
            deltaX = secondPressPos.x - firstPressPos.x;
            deltaY = secondPressPos.y - firstPressPos.y;
            

            print("First: " + firstPressPos.x + " " + firstPressPos.y);
            print("Second: " + secondPressPos.x + " " + secondPressPos.y + "\n");
            grow = true;
        }
    }


    //Called at start and is a slow progression loop
    private IEnumerator Grow(float extendSpeed)
    {
        //Waits until IsGrowing returns true;
        yield return new WaitUntil(IsGrowing);

        if (deltaX > deltaY)
        {
            for (int i = 0; i < extendFactor; i++)
            {
                //stretches object in the X direction
                gameObject.transform.localScale += new Vector3(extendSpeed, 0, 0);
                //moves the object along to accomidate for equal stretching on both sides
                gameObject.transform.Translate((extendSpeed) / 2, 0, 0);
                yield return new WaitForSeconds(extendSpeed / 10);
            }
        }
        else if (deltaX < deltaY)
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
}






