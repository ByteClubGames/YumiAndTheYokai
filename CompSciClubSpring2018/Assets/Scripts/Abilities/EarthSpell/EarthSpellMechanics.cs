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

    public float extendFactor = 25;
    public float extendSpeed = .1f;
    public int destroyTime = 5;

    public int maxSpells = 3;

    public bool earthGrowingY = false; //These are used to make use of the update function to grow the eSpell
    private int earthGrowingYIncrement = 0;

    //public bool earthGrowingNegY = false; //These are used to make use of the update function to grow the eSpell
    //private int earthGrowingNegYIncrement = 0;

    public bool earthGrowingX = false; //These are used to make use of the update function to grow the eSpell
    private int earthGrowingXIncrement = 0;





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
        
        //Once the earthspell starts growing, this if statement will run until the incrementor ends
        //This is to grow the eSpell every frame using the Update() function
        if (currentSwipe.y > 300 && currentSwipe.y != 0 || this.earthGrowingY)
        {
            this.earthGrowingY = true;
            if (this.earthGrowingY)
            {
                
                this.gameObject.transform.localScale += new Vector3(0, extendSpeed, 0);
                this.gameObject.transform.Translate(0, (extendSpeed) / 2, 0);
                earthGrowingYIncrement++;
                
                if (earthGrowingYIncrement == extendFactor)
                {
                    this.earthGrowingY = false;
                    earthGrowingYIncrement = 0;
                }
            }

            firstPressPos = new Vector2(0, 0);
            secondPressPos = new Vector2(0, 0);
            currentSwipe = new Vector2(0, 0);
        }

        ////Once the earthspell starts growing, this if statement will run until the incrementor ends
        ////This is to grow the eSpell every frame using the Update() function
        //if (currentSwipe.y < -300 && currentSwipe.y != 0 || this.earthGrowingNegY)
        //{
        //    this.earthGrowingNegY = true;
        //    if (this.earthGrowingNegY)
        //    {

        //        this.gameObject.transform.localScale += new Vector3(0, -extendSpeed, 0);
        //        this.gameObject.transform.Translate(0, -(extendSpeed) / 2, 0);
        //        earthGrowingNegYIncrement++;

        //        if (earthGrowingNegYIncrement == extendFactor)
        //        {
        //            this.earthGrowingNegY = false;
        //            earthGrowingNegYIncrement = 0;
        //        }
        //    }

        //    firstPressPos = new Vector2(0, 0);
        //    secondPressPos = new Vector2(0, 0);
        //    currentSwipe = new Vector2(0, 0);
        //}

        //Once the earthspell starts growing, this if statement will run until the incrementor ends
        //This is to grow the eSpell every frame using the Update() function
        if (currentSwipe.x > 300 && currentSwipe.x != 0 || this.earthGrowingX)
        {
            this.earthGrowingX = true;
            if (this.earthGrowingX)
            {

                this.gameObject.transform.localScale += new Vector3(extendSpeed, 0, 0);
                this.gameObject.transform.Translate((extendSpeed) / 2, 0, 0);
                earthGrowingXIncrement++;

                if (earthGrowingXIncrement == extendFactor)
                {
                    this.earthGrowingX = false;
                    earthGrowingXIncrement = 0;
                }
            }

            firstPressPos = new Vector2(0, 0);
            secondPressPos = new Vector2(0, 0);
            currentSwipe = new Vector2(0, 0);
        }



        //this is an attempt to use a different method of growing
        //if (currentSwipe.y > 300 && currentSwipe.y != 0)
        //{

        //    for (int i = 0; i < extendFactor; i++)
        //    { 
        //        this.gameObject.transform.localScale += new Vector3(0, extendSpeed, 0);
        //        this.gameObject.transform.Translate(0, (extendSpeed) / 2, 0);
        //        print(i);
        //        yield return new WaitForSeconds(.5f);

        //    }

        //    firstPressPos = new Vector2(0, 0);
        //    secondPressPos = new Vector2(0, 0);
        //    currentSwipe = new Vector2(0, 0);
        //}
    }


}










