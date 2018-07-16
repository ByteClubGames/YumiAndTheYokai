/* IceSpellAffectedEnemy.cs
 * Date Created: 3/17/18
 * Last Edited: 3/17/18
 * Programmer: Jack Bruce
 * Description: Added to all enemies that can be frozen.
 * IceSpellMechanics will use these methods to freeze enemies.
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class IceSpellAffectedEnemy : MonoBehaviour {

    public float freezeTime;
    private Stopwatch freezeTimer;

	// Use this for initialization
	void Start () 
    {
        freezeTimer = new Stopwatch();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (freezeTimer.ElapsedMilliseconds >= freezeTime * 1000)
        {
            UnFreeze();
            freezeTimer.Reset();
        }
	}

    public void Freeze ()
    {
        // will have to find a way to make this dynamic for other objects
        if (gameObject.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            freezeTimer.Start();

            //could freeze enemy more smoothly
            gameObject.GetComponent<BackAndForth>().isFrozen = true;

            //change enemy model to frozen enemy model

        }
    }

    private void UnFreeze ()
    {
            gameObject.GetComponent<BackAndForth>().isFrozen = false;

            //change frozen enemy model to unfrozen model
    }

}
