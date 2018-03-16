/* EarthSpellUse.cs
 * Date Created: 3/15/18
 * Last Edited: 3/15/18
 * Programmer: Daniel Jaffe
 * Description: Script for instantiating earth spell objects upon mouse clicks
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EarthSpellUse : MonoBehaviour {

    Vector3 playerinput = new Vector3(0, 0, 0); //gets player input
    public GameObject earthSpellTestObj; //will be used to check if the location is empty before spawning a cube in
    public GameObject earthSpell; //the actual cube of earth that will spawn into play
    public EarthSpellLocTest earthSpellTest; //to enable access to methods from the EarthSpellLocTest script


    private void Update()
	{
        //Instantiates earthSpellObj upon clicking        
        if (Input.GetMouseButtonDown(0)) {
            //get mouse position
            playerinput.x = Input.mousePosition.x;
            playerinput.y = Input.mousePosition.y;

            Instantiate(earthSpell, new Vector3(playerinput.x, playerinput.y, 0), Quaternion.identity);

            //drops in test cube without mesh to test for collisions
            //Instantiate(earthSpellTestObj, new Vector3(playerinput.x, playerinput.y, 0), Quaternion.identity);
            
            //if no collision, despawns the location test and spawns in an earthSpellObj -- if collision, still despawns the laction test object
            /*if (earthSpellTest.GetCollision() == false)
            {
                Destroy(earthSpellTestObj);
                Instantiate(earthSpell, new Vector3(playerinput.x, playerinput.y, 0), Quaternion.identity);
            } else
            {
                Destroy(earthSpellTestObj);
            }*/
        }
	}
}
