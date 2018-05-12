/* WindSpellMover.cs
 * Date Created: 5/08/18
 * Last Edited: 5/12/18
 * Programmer: Jack Bruce
 * Description: Moves 'WindSpell' obj on desired path
 *  - WindSpellSpawner will populate targetarray
 *  - once activated WindSpellUse will pass the target array to WindSpellMover (This is the current issue)
 *  - then WindSpell obj will follow path declared by array
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpellMover : MonoBehaviour {

	public float speed = 10;

	private Vector3[] wsTargets;
	private Vector3 nextPos;
	private int current;
	private int wsTargetsSize;

	// Use this for initialization
	void Start () {
		//wsTargets = GameObject.Find("WindSpellSpawner").GetComponent<WindSpellUse>().GetPositions();//I dont think you can pass an array like that
		wsTargetsSize = GameObject.Find("WindSpellSpawner").GetComponent<WindSpellUse>().GetTrgtAmnt();
		wsTargets = new Vector3[wsTargetsSize];
		for (int i = 0; i < wsTargets.Length; i++)
		{
			wsTargets[i] = GameObject.Find("WindSpellSpawner").GetComponent<WindSpellUse>().GetTarget(i);
		}
		current = 1;
	}
	
	// Update is called once per frame
	void Update () {
		nextPos = wsTargets[current];
		// move until you reach the current obj
		if (transform.position != nextPos)
		{
			Vector3 pos = Vector3.MoveTowards(transform.position,
			                                  nextPos, speed * Time.deltaTime);
			GetComponent<Rigidbody>().MovePosition(pos);
		}
		else if (current < wsTargets.Length)
			current++; // obj reached, move to the next obj
	}


}
