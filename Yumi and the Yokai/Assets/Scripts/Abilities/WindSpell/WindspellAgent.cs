/*WindSpellAgent.cs
********************************************************************************
*Creator(s).........................................................Darrell Wong
*Created................................................................12/14/18
*Last Modified...........................................................4/12/19
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

public class WindspellAgent : MonoBehaviour {

    public NavMeshAgent agent;
    private GameObject spawnerObj;
    private string wsSpawnerName = "WindSpellSpawner(Clone)"; //had to make this dynamic for instantiated prefab names
    public Vector3 velocity;

    public GameObject wind_impact;
    public ParticleSystem emit;

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
                    //print(transform.eulerAngles.z);
                    if (transform.eulerAngles.z > 100)        //this is here because a strange rotation bug with the navAgent was messing up the animation rotation
                                                              // eulerAngles only gives the angles 90 (for positive y) and 270 (negative y)
                    {
                        Instantiate(wind_impact, this.transform.position, Quaternion.Euler(0, 0, -this.transform.eulerAngles.x));
                    }
                    else
                    {
                        Instantiate(wind_impact, this.transform.position, Quaternion.Euler(0, 0, 180 + this.transform.eulerAngles.x));
                    }

                    DetachParticles();
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (  (!collision.gameObject.CompareTag("WindSpellTrigger") && collision.gameObject.name != "Yumi" && collision.gameObject.name != "PlayerDetection")
            || collision.gameObject.CompareTag("WindSpellAgent") || collision.gameObject.CompareTag("WindSpell"))
        {
            //spawnerObj.GetComponent<WindSpellUse>().CleanUp();
            print("collision with " + collision.gameObject.tag);
            Instantiate(wind_impact, this.transform.position, Quaternion.Euler(0, 0, -90 + this.transform.eulerAngles.z));
            DetachParticles();
            Destroy(gameObject);
        }
    }

    private void SetSpawner(GameObject spawner)
    {
        spawnerObj = spawner;
    }

    public void DetachParticles()
    {
        // This splits the particle off so it doesn't get deleted with the parent
        emit.transform.parent = null;

        // this stops the particle from creating more bits
        emit.enableEmission = false;
    }
}
