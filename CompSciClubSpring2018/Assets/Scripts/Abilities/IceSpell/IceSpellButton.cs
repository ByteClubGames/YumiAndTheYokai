/* IceSpellButton.cs
 * Date Created: 4/24/18
 * Last Edited: 5/08/18
 * Programmer: Jack Bruce
 * Description: Turns spell on and off.
 *  - 1st press spawns spell spawning obj
 *  - 2nd press destroys that object
 *  This can very easily be implemented to other spell buttons by changing the
 *  'spawner' variable to a prefab of your choosing.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class IceSpellButton : MonoBehaviour {

    public Button butt;
    public GameObject spawner;
    private GameObject spawnerInst;
    private bool spellOn;
    private int objCount;

	private void Start()
	{
        Button btn = butt.GetComponent<Button>();
        spellOn = false;
        btn.onClick.AddListener(TaskOnClick);
	}

    void TaskOnClick()
    {
        Debug.Log("You clicked the shit out of this button...");

        if (!spellOn) {
            spawnerInst = Instantiate(spawner);
            spellOn = true;
        } else {
            Destroy(spawnerInst);
            spellOn = false;
        }
    }
}
