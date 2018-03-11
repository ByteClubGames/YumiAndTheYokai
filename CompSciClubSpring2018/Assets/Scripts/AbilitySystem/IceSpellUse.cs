/* IceSpellUse.cs
 * Date Created: 3/10/18
 * Last Edited: 3/10/18
 * Programmer: Jack Bruce
 * Description: Script for instantiating ice spell objects upon mouse clicks
 */
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class IceSpellUse : MonoBehaviour {

    public GameObject iceSpellPrefab;
    private IceSpell iceS;
    private Stopwatch abilityCooldownTimer;

	private void Update()
	{
		//Instantiates iceSpellPrefab upon clicking in the position of the click
        if (Input.GetMouseButtonDown(0) )
        {
            Vector3 p = Camera.main.ScreenToWorldPoint(new 
                        Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            Instantiate(iceSpellPrefab, new Vector3(p.x, p.y, 0.0f), 
                        Quaternion.identity);
            

        }


	}

}
