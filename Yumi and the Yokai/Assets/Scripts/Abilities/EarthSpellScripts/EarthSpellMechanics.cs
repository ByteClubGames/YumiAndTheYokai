/*
 * ******************************************************************************************************
 * Creator(s).................................................................Daniel Jaffe & Darrell Wong
 * Created.....................................................................................03/15/2018
 * Last Modified..................................................................@ 10:59PM on 12/21/2018
 * Last Modified by..........................................................................Daniel Jaffe
 * 
 * 
 * Description:   Functionality of Earth Spell - Attach to the earthSpell object (cube):
 *                1. Earth spell is created from EarthSpellUse.cs where its properties are defined
 *                2. With line of sight, click will produce a pillar that grows normal to the surface
 *                3. Properties of growth speed/distance, maxSpells, and decay time are public variables
 *                4. Earth spell growth will stop on collision with specified objects with:
 *                   a)"Platforms" layer
 *                   b)"Earth" layer
 *                   c)"Earth Spell Object" tag
 *                5. EarthSpell starts with size 1,0,1. Grows to 1,1,1.
 * ******************************************************************************************************
 */

using System.Collections;
using UnityEngine;

public class EarthSpellMechanics : MonoBehaviour
{
    #region Global Variables
    public int maxSpells = 3;
    public int destroyTime = 5;
    float scalarvalue;
    #endregion

    private void Start()
    {
        Destroy(transform.parent.gameObject, destroyTime); //Destroy timer starts on creation
    }

    private void Update()
    {
        //destroy the earliest spell when there are too many
        if (GameObject.FindGameObjectsWithTag("Earth Spell Object").Length > maxSpells)
        {
            Destroy(GameObject.FindGameObjectsWithTag("Earth Spell Object")[0]);
        }
    }

    public void Initialize(float scalar)
    {
        transform.parent.gameObject.transform.localScale -= new Vector3(0, 0.3865392F * scalar, 0); //subtracts from the max length based on the scalar passed. A scaler of 0 means that it will not subtract any length.
    }
}