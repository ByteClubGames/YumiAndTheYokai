/* WindSpellMover.cs
 * Date Created: 5/08/18
 * Last Edited: 5/10/18
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

	private Transform[] targets = new Transform[100];
	private Transform nextPos;
	private int current;

	// Use this for initialization
	void Start () {
		WindSpellUse spellUse = new WindSpellUse();
		targets = spellUse.GetTransforms(); //this does not get the loaded array.. it is an empty array
		current = 0;
	}
	
	// Update is called once per frame
	void Update () {
		nextPos = targets[current];
		// move until you reach the current obj
		if (transform.position != nextPos.position)
		{
			Vector3 pos = Vector3.MoveTowards(transform.position,
			                                  nextPos.position, speed * Time.deltaTime);
			GetComponent<Rigidbody>().MovePosition(pos);
		}
		else current = (current + 1) % targets.Length; // obj reached, move to the next obj
	}


}
