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

}

