/* WindSpellMover.cs
 * Date Created: 5/08/18
 * Last Edited: 5/08/18
 * Programmer: Jack Bruce
 * Description: Moves 'WindSpell' obj on desired path
 *  - WindSpellSpawner will populate target array
 *  - once activated WindSpell will move to each position in array
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpellMover : MonoBehaviour {

	public Transform[] target;
	public float speed;

	private int current;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		// move until you reach the current obj
		if (transform.position != target[current].position)
		{
			Vector3 pos = Vector3.MoveTowards(transform.position,
											  target[current].position, speed * Time.deltaTime);
			GetComponent<Rigidbody>().MovePosition(pos);
		}
		else current = (current + 1) % target.Length; // obj reached, move to the next obj
	}
}
