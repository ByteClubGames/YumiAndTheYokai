/* WindSpellUse.cs
 * Date Created: 4/06/18
 * Last Edited: 4/10/18
 * Programmer: Jack Bruce && Stephen
 * Description: Modified from 'IceSpellUse.cs'
 * Attatch to WindSpellSpawner (This script is Active during "Draw Mode")
 *  -should spawn target objs (should be type 'Transform' instead of GameObject)
 *   upon dragging
 *  -makes array of Target object (Empty game objects just used for Transform)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpellUse : MonoBehaviour
{

	public GameObject targetPrefab; //empty gameobject used for Transform
	public GameObject player;
	public GameObject windSpellPrefab;
	public int trgtAmnt;

	private GameObject windSpell;
	private Transform[] targets;
	private int trgtNX;
	private bool drawMode;

	private bool _isDragging = false;

	// Use this for initialization
	void Start()
	{
		drawMode = true;
		trgtNX = 0;
		targets = new Transform[trgtAmnt];
		targets[trgtNX++] = player.transform;
	}

	// Update is called once per frame
	void Update()
	{

		//Once array is full OR Draw mode is manually turned off
		//Spawn WindSpell Object and load it with target array
		if (trgtNX >= targets.Length && drawMode)
		{
			drawMode = false;
			windSpell = Instantiate(windSpellPrefab, targets[0]); //spawn wind spell object @ player pos
		}

		if (Input.GetMouseButtonDown(0))
		{
			_isDragging = true;
		}
		if (Input.GetMouseButtonUp(0))
		{
			_isDragging = false;
			return;
		}
		if (_isDragging && drawMode) //SPAWNS WAY TO MANY OBJECTS AT ONCE! try per time
		{
			//Runs wind prefab is mouse button is pushed down
			Vector3 p = Camera.main.ScreenToWorldPoint(new
			Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
			Instantiate(targetPrefab, new Vector3(p.x, p.y, 0.0f), //spawns target
						Quaternion.identity);

			targetPrefab.transform.position = Input.mousePosition; //@ mouse position
			targets[trgtNX++] = targetPrefab.transform; //Adds position to target array


		}

	}

	public Transform[] GetTransforms() //called in WindSpellMover.cs to set positions
	{
		return targets;
	}

}
