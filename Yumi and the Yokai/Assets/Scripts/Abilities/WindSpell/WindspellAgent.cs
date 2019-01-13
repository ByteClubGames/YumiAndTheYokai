﻿/*WindSpellAgent.cs
********************************************************************************
*Creator(s).........................................................Darrell Wong
*Created................................................................12/14/18
*Last Modified..........................................................12/21/18
*Last Modified by...................................................Darrell Wong
*
*   Description: defines the windspell when it is using a nav mesh.
*
********************************************************************************
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WindSpellAgent : MonoBehaviour {

    public NavMeshAgent agent;
    private GameObject spawnerObj;
    private string wsSpawnerName = "WindSpellSpawner(Clone)"; //had to make this dynamic for instantiated prefab names
    public Vector3 velocity;

    void Awake () {
        agent = GetComponent<NavMeshAgent>();
        SetSpawner(GameObject.Find(wsSpawnerName));
    }

    private void Update()
    {
        velocity = agent.velocity;
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    spawnerObj.GetComponent<WindSpellUse>().CleanUp();
                    Destroy(gameObject);
                }
            }
        }
    }

    private void SetSpawner(GameObject spawner)
    {
        spawnerObj = spawner;
    }
}