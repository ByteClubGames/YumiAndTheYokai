/* ObjectSquish.cs
 * Date Created: 5/18/18
 * Last Edited: 5/18/18
 * Programmer: Darrell Wong
 * Description: to be attached to objects that need to be squished.
 *      1. When an object is squished, it will stop the earth spell growth and trigger an event i.e. stun enemy
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSquish : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision col)
    {
        //if (col.collider.gameObject.layer == LayerMask.NameToLayer("Floor") 
        //    && col.collider.gameObject.CompareTag("Earth Spell Object"))
            
        
    }
}
