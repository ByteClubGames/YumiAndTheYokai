/* EarthSpellUse.cs
 * Date Created: 3/15/18
 * Last Edited: 6/16/18
 * Programmer: Daniel Jaffe & Darrell Wong
 * Description: Spawn in the earthSpell object (cube) - Attach to the Earth Spell Spawner object:
 *      1. Uses a OverlapSphere to check eSpell overlap
 *      2. Spawns a eSpell object at the point of click
 *      
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EarthSpellUse : MonoBehaviour
{
    public float overlapRadius = .59F; // shpere to cover 1,1,1, cube
    public bool spellOverlap = false;
    public GameObject eSpell;
    public Vector3 playerinput = new Vector3(1, 1, 0); //gets player input

    void Start()
    {
        ////Jack put this here
        this.GetComponent<TimeManager>().StartSlowDown(); // Time is slowed when spawner is here
        ///
    }

    // Update is called once per frame
    void Update()
    {
        //execute on mouse click
        if (Input.GetMouseButtonDown(0))
        {
            //raycast from the camera and if ray hits something, then spawn eSpell
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Earth"))
            {
                //get the location of the player click
                Vector3 playerinput = new Vector3(hit.point.x, hit.point.y, 0);

                //Checking for overlapping eSpell spawns
                Collider[] hitColliders = Physics.OverlapSphere(playerinput, overlapRadius);
                spellOverlap = false;
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].tag == "Earth Spell Object")
                    {
                        print("Overlap");
                        spellOverlap = true;
                        break;
                    }
                }
                if (!spellOverlap)
                {
                    //spawns the eSpell object into play
                    Instantiate(eSpell, new Vector3(hit.point.x, hit.point.y, 0), Quaternion.identity);
                }

                //Swiper(); Swiper is now used in EarthSpellMechanics
            }
        }
    }
}


