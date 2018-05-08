/* IceSpellUse.cs
 * Date Created: 3/10/18
 * Last Edited: 3/10/18
 * Programmer: Jack Bruce
 * Description: Copied from 'IceSpellUse.cs'... this needs work
 * Attatch to WindSpellSpawner
 *  -should spawn target objs (should be type 'Transform' instead of GameObject)
 *   upon dragging
 *  -should add target objs to target array in 'WindSpellUse'
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpellUse : MonoBehaviour {
    
	public GameObject targetPrefab;
    private GameObject myCurrentObject;

    private bool _isDragging = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            return;
        }
        if (_isDragging)
        {
            //Runs ice prefab is mouse button is pushed down
            Vector3 p = Camera.main.ScreenToWorldPoint(new
              Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
			Instantiate(targetPrefab, new Vector3(p.x, p.y, 0.0f),
                        Quaternion.identity);
            targetPrefab.transform.position = Input.mousePosition;
        }

	}
}
