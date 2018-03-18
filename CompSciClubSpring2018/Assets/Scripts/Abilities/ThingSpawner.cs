/* ThingSpawner.cs
 * Date Created: 3/17/18
 * Last Edited: 3/17/18
 * Programmer: Jack Bruce
 * Description: Script for instantiating physics objects upon mouse clicks
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingSpawner : MonoBehaviour {
    
    public GameObject colliderPrefab;

    private void Update()
    {
        //Instantiates iceSpellPrefab upon clicking in the position of the click
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 p = Camera.main.ScreenToWorldPoint(new
                        Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            Instantiate(colliderPrefab, new Vector3(p.x, p.y, 0.0f),
                        Quaternion.identity);
        }


    }
}
