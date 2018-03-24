/* EarthSpellMechanics.cs
 * Date Created: 3/15/18
 * Last Edited: 3/17/18
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
    public float extendFactor = 3;
    private float deltaX, deltaY;



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
    }

    private void Update()
    {
        
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

        if (currentSwipe.y > 50 && currentSwipe.y != 0)
        {
            eSpell.transform.Translate(transform.up * Time.deltaTime * extendFactor);
            eSpell.transform.localScale += new Vector3(secondPressPos.x - firstPressPos.x, 0, 0);


            firstPressPos = new Vector2(0, 0);
            secondPressPos = new Vector2(0, 0);
            currentSwipe = new Vector2(0, 0);
        }
    }


}

