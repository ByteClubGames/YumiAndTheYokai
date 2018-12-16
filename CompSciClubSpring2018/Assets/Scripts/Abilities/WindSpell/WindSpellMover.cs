/* WindSpellMover.cs
 * Date Created: 5/08/18
 * Last Edited: 5/19/18
 * Programmer: Jack Bruce and Evanito
 * Description: Moves 'WindSpell' obj on desired path
 *  - WindSpellSpawner will populate targetarray
 *  - once activated WindSpellUse will pass the target LinkLis to WindSpellMover
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
	private string wsSpawnerName = "WindSpellSpawner(Clone)"; //had to make this dynamic for instantiated prefab names
    private GameObject spawnerObj;

	// Use this for initialization
	void Start () {
        SetSpawner(GameObject.Find(wsSpawnerName));
		wsTargets = spawnerObj.GetComponent<WindSpellUse>().GetTargets();
        wsTargetsSize = wsTargets.Length;
        NoiseReduction(wsTargets);
        current = 0;
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.CompareTag("WindSpellTrigger") && collision.gameObject.name != "Player") {
            spawnerObj.GetComponent<WindSpellUse>().CleanUp();
        }
    }

    private void SetSpawner(GameObject spawner)
    {
        spawnerObj = spawner;
    }

    // Update is called once per frame
    void Update () {
        if (transform.position == wsTargets[wsTargetsSize - 1])
        { // die at end of path (transform.position == wsTargets[wsTargets.Length - 1])
            try
            {
                spawnerObj.GetComponent<WindSpellUse>().CleanUp();
            }
            catch
            {
                Destroy(gameObject);
            }

        }

        nextPos = wsTargets[current];
        // move until you reach the current obj
        if (transform.position != nextPos)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(pos);
        }
        else if (current < wsTargetsSize - 1)
        {
            current++; // obj reached, move to the next obj
        }
		
	}

    private bool isZero(Vector3 inpos)
    {
        if (inpos.x == 0 && inpos.y == 0)
        {
            return true;
        }
        else
            return false;
    }

    private void NoiseReduction(Vector3[] src, int severity = 1)
    {
        for (int i = 1; i < src.Length; i++)
        {
            //---------------------------------------------------------------avg
            var start = (i - severity > 0 ? i - severity : 0);
            var end = (i + severity < src.Length ? i + severity : src.Length);

            float sumx = 0, sumy = 0;

            for (int j = start; j < end; j++)
            {
                sumx += src[j].x;
                sumy += src[j].y;
            }

            var avgx = sumx / (end - start);
            var avgy = sumy / (end - start);
            src[i].x = avgx;
            src[i].y = avgy;

        }
    }
}

