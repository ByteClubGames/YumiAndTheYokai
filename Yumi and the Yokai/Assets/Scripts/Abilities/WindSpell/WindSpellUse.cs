/*WindSpellUse.cs
********************************************************************************
*Creator(s).....................Jack Bruce && Stephen && Evanito && Darrell Wong
*Created.................................................................5/06/18
*Last Modified..........................................................4/12/2019
*Last Modified by...................................................Darrell Wong
*
*Attatch to WindSpellSpawner (This script is Active during "Draw Mode")
*        Description: Modified from 'IceSpellUse.cs'
*               -Spawns trgtObjs upon clicking
*               -makes array of trgtObjs position (Vector3)
*  
*               -differentiates between a click and a drag
*               -a click will activate the nav mesh ai to direct the windspell to the clicked point
*               -dragging will have the windspell follow the dragged path
********************************************************************************
*/
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;


public class WindSpellUse : MonoBehaviour
{

    public GameObject targetPrefab; //empty gameobject used for Transform
    private GameObject player;
    public GameObject windSpellPrefab;              //these windspells are different. One is for the drawn windspell
    public GameObject windPortalPrefab;
    private GameObject portal;                      //instantiated portal
    public NavMeshAgent windSpellAgentPrefab;       //this one is for the single click windspell
    public double minDeltaDis;
    public int drawSeconds = 10;
    //public GameObject timeManager;


    private GameObject windSpell;
    private bool isCurrentWindSpell;
    private bool isActivePortal;
    private NavMeshAgent windSpellAgent;
    private GameObject tempTarget;
    private TargList targets;
    private bool drawMode;
    private bool _isDragging = false;
    private bool _isDone;
    private Vector3 currentPos;
    private Stopwatch drawTimer = new Stopwatch();

    void Start()
    {
        drawMode = true;
        player = GameObject.Find("Yumi");
        windSpell = null;
        windSpellAgent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCurrentWindSpell)
        { 

            if (Input.GetMouseButtonDown(0) && !_isDragging)
            {
                _isDragging = true;
                drawTimer.Start();
                targets = new TargList();                     //create a new targList to reset the path for new windspells
                targets.addTarg(player.transform.position);   //look at this for the starting position of windspell

                if (!isCurrentWindSpell)
                {
                    portal = Instantiate(windPortalPrefab, player.transform.position, Quaternion.identity);          //spawn windspell portal
                    isActivePortal = true;
                }
            }
            if (Input.GetMouseButtonUp(0) && drawTimer.IsRunning)
            {
                _isDone = true;
                _isDragging = false;
                return;
            }
            if (_isDragging && drawMode)
            {
                //Runs wind prefab is mouse button is pushed down
                Vector3 p = Camera.main.ScreenToWorldPoint(new
                Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                double dist = GetDistance(p.x, p.y, targets.getBottom().x, targets.getBottom().y);
                if (dist >= minDeltaDis) // check if distance changed enough || that enough time has passed ??
                {
                    tempTarget = Instantiate(targetPrefab, new Vector3(p.x, p.y, 0.0f), //spawns target
                        Quaternion.identity);

                    //particleList[particleCount++] = tempTarget;

                    targets.addTarg(tempTarget.transform.position); //Adds position to target array
                                                                    //Destroys all target objects immediately after spawning.
                                                                    //You may want to disable this for tracking how many targets spawn.
                                                                    //Hell, it may not even matter how many objects spawn if I delete them so dang fast
                                                                    //tempTarget.make sprite

                    //Destroy(tempTarget); 

                    //I commented this out because I need the temp targets so I can use particle system.
                    //tempTargets get destroyed the moment you let go of the mouse button and the windspell is instantiated
                    //      -the wind spell will call getArray whitch cleans up the targetList
                }
            }

            //if only clicked for less than half a second it will spawn a (navmesh) windspell agent
            if ((_isDone && drawMode) && drawTimer.ElapsedMilliseconds < drawSeconds * 50)
            {
                drawTimer.Stop();
                if (windSpellAgent == null && !isCurrentWindSpell)        //prevents multiple windspells
                {
                    windSpellAgent = Instantiate(windSpellAgentPrefab, player.transform.position + new Vector3(0f, .2f, 0f), Quaternion.identity); //spawn wind spell AGENT object @ player pos
                                                                                                                                                   //windspellAgent.tag = "WindSpellAgent";

                    Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));

                    windSpellAgent.SetDestination(new Vector3(p.x, p.y, 0f));

                }
                resetWindSpells();
            }

            // if held down for more than ~0.5 seconds it will spawn a (drawn) windSpell
            else if ((_isDone && drawMode) || drawTimer.ElapsedMilliseconds > drawSeconds * 1000)
            {
                drawTimer.Stop();
                drawTimer.Reset();
                _isDone = false;

                if (windSpell == null && !isCurrentWindSpell)
                {
                    windSpell = Instantiate(windSpellPrefab, targets.getTop(), Quaternion.identity); //spawn wind spell object @ player pos
                    windSpell.tag = "WindSpell";
                    windSpell.SendMessage("SetSpawner", gameObject);
                }
                resetWindSpells();
            }
        }


        if (isCurrentWindSpell && !_isDragging)               //when done drawing the spell's path destroy all targets
        {
            foreach (GameObject target in GameObject.FindObjectsOfType(typeof(GameObject)))         //find all targets with name windspelltarget(clone)
            {
                if (target.name == "WindSpellTarget(Clone)")
                {
                    GameObject emitter = target.transform.Find("trailEmitter").gameObject;          //get the trailEmitter child object of each target

                    ParticleSystem emit = emitter.GetComponent<ParticleSystem>();                   //get the ParticleSystem object of the trailEmitter

                    emit.transform.parent = null;                                                    //detach particles from the target
                    emit.enableEmission = false;

                    Destroy(emitter, 3);                //destroy emitter after a delay so that the particles will disappear naturally
                    Destroy(target);                    //clean up the targets
                }

            }
        }

        if (GameObject.Find("WindSpell(Clone)") != null || GameObject.Find("WindSpellAgent(Clone)") != null)
        {
            isCurrentWindSpell = true;
        }
        else
        {
            isCurrentWindSpell = false;
        }
    }

    public void resetWindSpells()
    {
        drawTimer.Reset();
        //windSpellAgent = null;    //enable this if you want a lot of windspells
        _isDone = false;
        drawMode = true;

        //StartCoroutine("DeletePortal");
        DetachParticles(portal);
    }

    //EITHER USE THIS SHITTTTTTTTTTTTTTTTTTTTTTT OR CLEAN IT UUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUP
    //IEnumerator DeletePortal()
    //{
    //    GameObject deletePortal = portal;
    //    portal = null;
    //    yield return new WaitForSeconds(.5f);

    //    DetachParticles(deletePortal);

    //    //Destroy(deletePortal, 3);
    //}

    public void DetachParticles(GameObject portal)
    {
        GameObject emitter = portal.transform.GetChild(0).gameObject;

        ParticleSystem emit = emitter.GetComponent<ParticleSystem>();

        // This splits the particle off so it doesn't get deleted with the parent
        emit.transform.parent = null;

        // this stops the particle from creating more bits
        emit.enableEmission = false;

        isActivePortal = false;
        Destroy(this.portal);
    }

    public Vector3[] GetTargets()
    {
        return targets.getArray();
    }

    public double GetDistance(double x1, double y1, double x2, double y2)
    {
        double inside = (System.Math.Pow((x1 - x2), 2) + System.Math.Pow((y1 - y2), 2));
        double distance = System.Math.Sqrt(inside);
        //distance = distance < 0 ? -distance : distance; // absolute value
        return distance;
    }

    public void CleanUp()
    {
        Destroy(windSpell);
        //Destroy(gameObject);
        //Start();
    }
}


// LIST CODE
public class TargList
{
    private TargLink top = null;
    private TargLink bottom = null;
    private int length;


    public TargList()
    {
        top = null;
        bottom = null;
        length = 0;
    }

    public void addTarg(TargLink inlink)
    {
        if (top == null)
        {
            top = inlink;
        }
        else
        {
            bottom.setNext(inlink);
        }
        bottom = inlink;
        length++;
    }

    public void addTarg(Vector3 invect)
    {
        TargLink temp = new TargLink(invect);
        addTarg(temp);
    }

    public Vector3[] getArray()
    {
        Vector3[] arry = new Vector3[length];
        TargLink cur = top;
        for (int i = 0; i < length; i++)
        {
            arry[i] = cur.getData();
            //cur.deleteEmitter();
            cur = cur.getNext();
        }
        return arry;
    }


    public TargLink getFirst()
    {
        return top;
    }

    public Vector3 getTop()
    {
        return top.getData();
    }

    public Vector3 getBottom()
    {
        return bottom.getData();
    }

    public void deleteTargetList()
    {
        top = null;
    }
}

public class TargLink
{
    private Vector3 current;
    private TargLink next = null;

    public TargLink(Vector3 intemp)
    {
        current = intemp;
    }

    public void setNext(TargLink nexto)
    {
        next = nexto;
    }

    public Vector3 getData()
    {
        return current;
    }

    public TargLink getNext()
    {
        return next;
    }
}