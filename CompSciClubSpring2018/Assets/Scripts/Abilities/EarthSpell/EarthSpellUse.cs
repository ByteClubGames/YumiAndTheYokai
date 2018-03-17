/* EarthSpellUse.cs
 * Date Created: 3/15/18
 * Last Edited: 3/17/18
 * Programmer: Daniel Jaffe
 * Description: Spawn in the earthSpell object (cube) - Attach to the Earth Spell Spawner object:
 *      1. Spawns a eSpell object at the point of click
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSpellUse : MonoBehaviour
{

    public GameObject eSpell;
    Vector3 playerinput = new Vector3(1, 1, 0); //gets player input

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //execute on mouse click
        if (Input.GetMouseButtonDown(0))
        {
            //get the location of the player click
            Vector3 playerinput = Camera.main.ScreenToWorldPoint(new
                        Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            //spawns the eSpell object into play
            Instantiate(eSpell, new Vector3(playerinput.x, playerinput.y, 0), Quaternion.identity);
        }
    }
}
