/* WindSpellUse.cs
 * Date Created: 5/06/18
 * Last Edited: 5/12/18
 * Programmer: Jack Bruce && Stephen
 * Description: Modified from 'IceSpellUse.cs'
 * Attatch to WindSpellSpawner (This script is Active during "Draw Mode")
 *  -Spawns trgtObjs upon clicking
 *  -makes array of trgtObjs position (Vector3)
 */
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WindSpellUse : MonoBehaviour
{

	public GameObject targetPrefab; //empty gameobject used for Transform
	public GameObject player;
	public GameObject windSpellPrefab;
	public int trgtAmnt, dTime;
    
	private GameObject windSpell;
	private Vector3[] targets;
	private int trgtNX;
	private bool drawMode;
	private int lastTime;
	private bool _isDragging = false;
	private Vector3 currentPos;

	// Use this for initialization
	void Start()
	{
		drawMode = true;
		trgtNX = 1;
		targets = new Vector3[trgtAmnt];
		targets[0] = player.transform.position;
	}

	// Update is called once per frame
	void Update()
	{

		//Once array is full OR Draw mode is manually turned off
		//Spawn WindSpell Object and load it with target array
		if (trgtNX >= targets.Length && drawMode)
		{
			drawMode = false;
			windSpell = Instantiate(windSpellPrefab, targets[0], Quaternion.identity); //spawn wind spell object @ player pos
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
			Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f)); //THERE MUST BE SOMETHING WRONG WITH THIS...B/C ARRAY VALUES ARE WHACK
			Instantiate(targetPrefab, new Vector3(p.x, p.y, 0.0f), //spawns target
						Quaternion.identity);
			
			targetPrefab.transform.position = Input.mousePosition; //@ mouse position
			targets[trgtNX] = targetPrefab.transform.position; //Adds position to target array
			currentPos = targets[trgtNX];
			trgtNX++;

            
		}

	}

	public Vector3[] GetTargets() //called in WindSpellMover.cs to set positions
	{
		return targets;
	}

	public Vector3 GetTarget(int NX)
	{
		return targets[NX];
	}

    public int GetTrgtAmnt()
	{
		return trgtAmnt;
	}
}
