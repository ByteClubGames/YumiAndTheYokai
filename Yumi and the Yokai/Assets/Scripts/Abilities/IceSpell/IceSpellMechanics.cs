/* IceSpellMechanics.cs
 * Date Created: 3/10/18
 * Last Edited: 3/17/18
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
    
    public float lifeTime;
    //public float freezeTime;
    private Stopwatch freezeTimer = new Stopwatch();
    private GameObject frozenEnemy;


	// Use this for initialization
	void Start () 
    {
        Destroy(gameObject, lifeTime); //Destroys IceSpell object after lifeTime
	}
	
	// Update is called once per frame
	void Update () 
    {
      
        //if (freezeTimer.ElapsedMilliseconds >= freezeTime * 999)
        //{
        //    UnFreezeEnemy(frozenEnemy);
        //    print("yo");
        //    freezeTimer.Reset();
        //}

	}

    // called when there is a collision (make sure IceSpell isTrigger)
	private void OnTriggerEnter(Collider col)
	{
        if (col.gameObject.name == "Test Enemy")
        {
            col.gameObject.GetComponent<IceSpellAffectedEnemy>().Freeze();
        }

        if (col.gameObject.tag == "Water")
        {
            col.gameObject.GetComponent<IceSpellAffectedSurface>().Freeze();
        }
        //FreezeEnemy(col.gameObject);
        //FreezeWater(col.gameObject);

        //FreezeEnemy(col.gameObject);
        //FreezeWater(col.gameObject);

	}


    /* I PUT THESE METHODS IN "IceSpellAffectedEnemy.cs" THEY MAKE MORE SENSE
     * TO BE ATTACHED TO THE OBJECT THAT IS BEING FROZEN
     * 
	//freezes collided object if it is moving
	private void FreezeEnemy(GameObject enemy) 
    {
        
        // will have to find a way to make this dynamic for other objects
        if (enemy.name == "Test Enemy" &&
            enemy.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            freezeTimer.Start();

            //could freeze enemy more smoothly
            enemy.GetComponent<BackAndForth>().isFrozen = true; 

            //change enemy model to frozen enemy model

            frozenEnemy = enemy; // used to access collided object in update
        }

    }

<<<<<<< Updated upstream
    private void UnFreezeEnemy(GameObject enemy)
    {
        if (enemy.name == "Test Enemy")
        {

            enemy.GetComponent<BackAndForth>().isFrozen = false;

            //change frozen enemy model to unfrozen model

        }
    }


    private void FreezeWater(GameObject water)
    {
        //technically melts... lol 
        //I need a test player to test the reverse
        //isTrigger will need to be set to false for freeze
        if (water.gameObject.tag == "Water")
        {
            (water.GetComponent(typeof(Collider)) as Collider).isTrigger = true;
        }

=======
    private void FreezeWater (GameObject water) 
    {
        if (water.gameObject.tag == "Water"){
            
            (water.GetComponent(typeof(Collider)) as Collider).isTrigger = true;
        }

        //if there is a tile colliding with IceSpell
        //setActive
        //Check if it is a 'water' tile
        //and turn it into an 'ice' tile

        //after 'freezeTime' has elapsed 'unfreeze'
        //change 'ice' tile back to 'water' tile
>>>>>>> Stashed changes
    }
    *
    *
    */

}
