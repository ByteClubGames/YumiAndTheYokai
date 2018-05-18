/* WindSpellUse.cs
 * Date Created: 5/06/18
 * Last Edited: 5/12/18
 * Programmer: Jack Bruce && Stephen && Evanito
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
    public double minDeltaDis;
    
	private GameObject windSpell;
	private GameObject tempTarget;
	private Vector3[] targets;
	private int trgtNX;
	private bool drawMode;
	private bool _isDragging = false;
    private bool _isDone;
	private Vector3 currentPos;

	// Use this for initialization
	void Start()
	{
		drawMode = true;
        _isDone = false;
        trgtNX = 1;
		targets = new Vector3[trgtAmnt];
		targets[0] = player.transform.position;
        windSpell = null;
    }

	// Update is called once per frame
	void Update()
	{

		//Once array is full OR Draw mode is manually turned off
		//Spawn WindSpell Object and load it with target array
		if (_isDone || (trgtNX >= targets.Length && drawMode))
		{
            _isDone = false;
			drawMode = false;
            if (windSpell == null)
            {
                windSpell = Instantiate(windSpellPrefab, targets[0], Quaternion.identity); //spawn wind spell object @ player pos
                windSpell.tag = "WindSpell";
            }
            //And Destroy all target objects
		}

		if (Input.GetMouseButtonDown(0))
		{
			_isDragging = true;
		}
		if (Input.GetMouseButtonUp(0))
		{
            _isDone = true;
			_isDragging = false;
			return;
		}
		if (_isDragging && drawMode) //SPAWNS WAY TO MANY OBJECTS AT ONCE! try per time
		{
			
			//Runs wind prefab is mouse button is pushed down
			Vector3 p = Camera.main.ScreenToWorldPoint(new
			Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f)); //THERE MUST BE SOMETHING WRONG WITH THIS...B/C ARRAY VALUES ARE WHACK
            float dist = GetDistanceFast(p.x, p.y, targets[trgtNX - 1].x, targets[trgtNX - 1].y);
            if (dist >= minDeltaDis) // check if distance changed enough || that enough time has passed
            {
                tempTarget = Instantiate(targetPrefab, new Vector3(p.x, p.y, 0.0f), //spawns target
                    Quaternion.identity);
                //targetPrefab.transform.position = Input.mousePosition; //@ mouse position
                targets[trgtNX] = tempTarget.transform.position; //Adds position to target array
                currentPos = targets[trgtNX];
                trgtNX++;
                //Destroys all target objects immediately after spawning.
                //You may want to disable this for tracking how many targets spawn.
                //Hell, it may not even matter how many objects spawn if I delete them so dang fast
                Destroy(tempTarget);
            }
            
		}

	}

	public Vector3 GetTarget(int NX)
	{
		return targets[NX];
	}

    public Vector3[] GetTargets()
    {
        return targets;
    }

    public int GetTrgtAmnt()
	{
		return trgtAmnt;
	}

    public float GetDistanceFast(float x1, float y1, float x2, float y2)
    {
        float distance = x1 + y1 - x2 - y2; // Is fast because it gives a relative distance without using CPU heavy sqrt()
        distance = distance < 0 ? -distance : distance; // absolute value
        return distance;
    }

    public void CleanUp()
    {
        Destroy(windSpell);
        Start();
    }
}
