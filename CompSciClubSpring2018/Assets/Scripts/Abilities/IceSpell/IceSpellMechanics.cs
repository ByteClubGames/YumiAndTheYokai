/* IceSpellMechanics.cs
 * Date Created: 3/10/18
 * Last Edited: 3/11/18
 * Programmer: Jack Bruce
 * Description: Functionality of Ice Spell:
 *      1. Freeze enemies within collider (for a limited time)
 *      2. Change 'Water' tiles to 'Ice' Tiles (for a limited time)
 *      3. Disappear when finger leaves (multiple 'IceSpell' objects will be
 *          spawned so we will use a StopWatch and lifeTime var)
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class IceSpellMechanics : MonoBehaviour {

    // public vars for testing
    public float lifeTime;
    public float freezeTime;
    //

    private Stopwatch freezeTimer = new Stopwatch();


	// Use this for initialization
	void Start () 
    {
        Destroy(gameObject, lifeTime); //Destroys IceSpell object after lifeTime
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //called when there is a collision
	private void OnCollisionEnter(Collision col)
	{

        FreezeEnemy(col.gameObject);



	}

    //freezes collided object if it is moving
    private void FreezeEnemy(GameObject enemy) 
    {

        // will have to find a way to make this dynamic for other objects
        if (enemy.name == "TestEnemy" &&
            enemy.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            //stops enemy very unnaturally
            //try stopping moving objects without directly accessing speed/vel
            enemy.GetComponent<BackAndForth>().speed = 0;

            //change enemy model to frozen enemy model

            //after 'freezeTime' has elapsed 'unfreeze'
                //change frozen enemy model to enemy model
                //restore objects origninal speed
        }

    }

    private void FreezeTile (GameObject tile) 
    {
        tile.GetType();
        //if there is a tile colliding with IceSpell

            //Check if it is a 'water' tile
            //and turn it into an 'ice' tile

            //after 'freezeTime' has elapsed 'unfreeze'
                //change 'ice' tile back to 'water' tile
    }

}
