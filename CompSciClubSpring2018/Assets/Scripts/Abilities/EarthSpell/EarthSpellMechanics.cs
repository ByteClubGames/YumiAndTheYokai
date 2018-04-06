/* EarthSpellMechanics.cs
 * Date Created: 3/15/18
 * Last Edited: 3/29/18
 * Programmer: Daniel Jaffe
 * Description: Functionality of Earth Spell - Attach to the earthSpell object (cube):
 *      1. Tests if there is collision with another gameobject. If so, despawns.
 *      2. Not yet implemented: Spawns in without mesh layer. If collision == false, then turn mesh layer on. 
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EarthSpellMechanics : MonoBehaviour {


    public Vector2 firstPressPos = new Vector2(0, 0);
    public Vector2 secondPressPos;
    public Vector2 currentSwipe;
    private float deltaX, deltaY;
    public float swipeThreshold;

    public float extendFactor = 25;
    public float extendSpeed = .1f;
    public int destroyTime = 5;

    public int maxSpells = 3;

    //These earthGrowing variables are used to make use of the update function to grow the eSpell
    public bool earthGrowingY = false; //Positive Y growth
    private int earthGrowingYIncrement = 0;

    public bool earthGrowingNegY = false; //Negative Y growth
    private int earthGrowingNegYIncrement = 0;

    public bool earthGrowingX = false; //Positive X growth
    private int earthGrowingXIncrement = 0;

    public bool earthGrowingNegX = false; //Negative X growth
    private int earthGrowingNegXIncrement = 0;



    /***Resolved in Earth Spell Use***
    Collision col = new Collision(); //collision object needed to call OnCollisionEnter method

    //if called, will destroy object if collision is detected and if not an Earth Spell Object
    void OnCollisionEnter(Collision col)
    {
        if ((col.gameObject.layer != LayerMask.NameToLayer("Earth")))
        {
            Destroy(gameObject);
        }
    }*/


    private void Start () 
    {
        

        /***Resolved in Earth Spell Use***
        //check for collision
        OnCollisionEnter(col);*/
        Destroy(gameObject, destroyTime); //Destroy timer starts on creation
        
    }

    private void Update()
    {
        Swiper();
        
        if (GameObject.FindGameObjectsWithTag("Earth Spell Object").Length > maxSpells) //destroy the earliest spell when there are too many
        { 
            Destroy(GameObject.FindGameObjectsWithTag("Earth Spell Object")[0]);
        }
    }

    private void Swiper()
    {
        //get user input
        if (Input.GetMouseButtonDown(0))
        {
            //get first mouse position
            firstPressPos.x = Input.mousePosition.x;
            firstPressPos.y = Input.mousePosition.y;

        }
        if (Input.GetMouseButtonUp(0))
        {
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            deltaX = secondPressPos.x - firstPressPos.x;
            deltaY = secondPressPos.y - firstPressPos.y;

            currentSwipe = new Vector2(deltaX, deltaY);
        }


        //Decides which direction to grow by setting the earthGrowing boolean to true
        print(Mathf.Abs(currentSwipe.y) + " " + Mathf.Abs(currentSwipe.x));
        if (Mathf.Abs(currentSwipe.y) > Mathf.Abs(currentSwipe.x)) //Check if abs value of y or x is greater.
        {
            if (currentSwipe.y > swipeThreshold)
            {
                earthGrowingY = true;
                print("1");
            }
            else //if (currentSwipe.y < -swipeThreshold)
            {
                earthGrowingNegY = true;
                print("2");
            }
        }
        else if (Mathf.Abs(currentSwipe.x) > Mathf.Abs(currentSwipe.y))
        {
            if (currentSwipe.x > swipeThreshold)
            {
                earthGrowingX = true;
                print("3");
            }
            if (currentSwipe.x < -swipeThreshold)
            {
                earthGrowingNegX = true;
                print("4");
            }
        }


        /*
        grows the earthSpell depending on the chosen direction
        */
        if (earthGrowingY)
        {

            gameObject.transform.localScale += new Vector3(0, extendSpeed, 0);
            gameObject.transform.Translate(0, (extendSpeed) / 2, 0);
            earthGrowingYIncrement++;
                
            if (earthGrowingYIncrement == extendFactor)
            {
                earthGrowingY = false; //once done growing, resets these values
                earthGrowingYIncrement = 0;
            }

            firstPressPos = new Vector2(0, 0);
            secondPressPos = new Vector2(0, 0);
            currentSwipe = new Vector2(0, 0);
        }



        /*
         Attempt to grow in the negative directions.
         */
        if (earthGrowingNegY)
        {
            gameObject.transform.localScale += new Vector3(0, extendSpeed, 0);
            gameObject.transform.Translate(0, -(extendSpeed) / 2, 0);
            earthGrowingNegYIncrement++;

            if (earthGrowingNegYIncrement == extendFactor)
            {
                earthGrowingNegY = false;
                earthGrowingNegYIncrement = 0;
            }


            firstPressPos = new Vector2(0, 0);
            secondPressPos = new Vector2(0, 0);
            currentSwipe = new Vector2(0, 0);
        }


        if (earthGrowingX)
        {

            gameObject.transform.localScale += new Vector3(extendSpeed, 0, 0);
            gameObject.transform.Translate((extendSpeed) / 2, 0, 0);
            earthGrowingXIncrement++;

            if (earthGrowingXIncrement == extendFactor)
            {
                earthGrowingX = false;
                earthGrowingXIncrement = 0;
            }


            firstPressPos = new Vector2(0, 0);
            secondPressPos = new Vector2(0, 0);
            currentSwipe = new Vector2(0, 0);
        }

        if (earthGrowingNegX)
        {

            gameObject.transform.localScale += new Vector3(extendSpeed, 0, 0);
            gameObject.transform.Translate((-extendSpeed) / 2, 0, 0);
            earthGrowingNegXIncrement++;

            if (earthGrowingNegXIncrement == extendFactor)
            {
                earthGrowingNegX = false;
                earthGrowingNegXIncrement = 0;
            }


            firstPressPos = new Vector2(0, 0);
            secondPressPos = new Vector2(0, 0);
            currentSwipe = new Vector2(0, 0);
        }


        /*
        this is an attempt to use a different method of growing
        it didnt work :(
        it is supposed to use WaitForSeconds which required modification 
        to the headers with the IEnumerator
         */

        //if (currentSwipe.y > 300 && currentSwipe.y != 0)
        //{

        //    for (int i = 0; i < extendFactor; i++)
        //    { 
        //        gameObject.transform.localScale += new Vector3(0, extendSpeed, 0);
        //        gameObject.transform.Translate(0, (extendSpeed) / 2, 0);
        //        print(i);
        //        yield return new WaitForSeconds(.5f);

        //    }

        //    firstPressPos = new Vector2(0, 0);
        //    secondPressPos = new Vector2(0, 0);
        //    currentSwipe = new Vector2(0, 0);
        //}
    }

    
}










