/* EarthSpellLocTest.cs
 * Date Created: 3/15/18
 * Last Edited: 3/15/18
 * Programmer: Daniel Jaffe
 * Description: Earth Spell Location Test:
 *      1. Spawn in an empty game object with the same dimentions as the earthSpellObj
 *      2. Set boolean value true if collision is present
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSpellLocTest : MonoBehaviour {
    public bool collision;
	
    //returns true if collision is detected and false if not
    public bool GetCollision()
    {
        return collision;
    }
    
    //method to check if there is a collision and then adjust the collision value accordingly
    public void SetCollision()
    {
        collision = true;
    }

    //check if there is a collision and then adjust the collision boolean accordingly
    public void OnCollisionEnter(Collision collision)
    {
        SetCollision();
    }
}
