/* IceSpellAffectedSurface.cs
 * Date Created: 3/17/18
 * Last Edited: 3/17/18
 * Programmer: Jack Bruce
 * Description: Added to all surfaces that can be frozen aka WATER.
 * IceSpellMechanics will use these methods to freeze water surfaces and make
 * them able to be walked across.
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class IceSpellAffectedSurface : MonoBehaviour {

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
        freezeTimer.Start();
        (gameObject.GetComponent(typeof(Collider)) as Collider).isTrigger = false;
    }

    public void UnFreeze ()
    {
        (gameObject.GetComponent(typeof(Collider)) as Collider).isTrigger = true;
    }

}
