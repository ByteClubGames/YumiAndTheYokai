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
