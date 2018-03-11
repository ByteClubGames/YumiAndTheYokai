/* IceSpellMechanics.cs
 * Date Created: 3/10/18
 * Last Edited: 3/10/18
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

	private void OnCollisionEnter(Collision col)
	{

        // will have to find a way to make this dynamic for other objects
        if (col.gameObject.name == "TestEnemy" && 
            col.gameObject.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            //stops enemy very unnaturally
            col.gameObject.GetComponent<BackAndForth>().speed = 0;

        }
	}
}
