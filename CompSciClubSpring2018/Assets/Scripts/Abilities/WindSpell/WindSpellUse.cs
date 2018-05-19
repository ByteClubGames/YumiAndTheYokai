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
	private GameObject player;
	public GameObject windSpellPrefab;
    public double minDeltaDis;
    
	private GameObject windSpell;
	private GameObject tempTarget;
	private TargList targets;
	private bool drawMode;
	private bool _isDragging = false;
    private bool _isDone;
	private Vector3 currentPos;

	// Use this for initialization
	void Start()
	{
		drawMode = true;
        _isDone = false;
		targets = new TargList();
        player = GameObject.Find("Player");
        targets.addTarg(player.transform.position);
        windSpell = null;
    }

	// Update is called once per frame
	void Update()
	{

		//Once array is full OR Draw mode is manually turned off
		//Spawn WindSpell Object and load it with target array
		if (_isDone && drawMode) // also add time draw check? currently no length limit
		{
            _isDone = false;
			drawMode = false;
            if (windSpell == null)
            {
                windSpell = Instantiate(windSpellPrefab, targets.getTop(), Quaternion.identity); //spawn wind spell object @ player pos
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
                targets.addTarg(tempTarget.transform.position); //Adds position to target array
                //Destroys all target objects immediately after spawning.
                //You may want to disable this for tracking how many targets spawn.
                //Hell, it may not even matter how many objects spawn if I delete them so dang fast
                //tempTarget.make sprite
                Destroy(tempTarget);
            }
		}
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
        Start();
    }
}

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

    public void addTarg (TargLink inlink)
    {
        if (top == null )
        {
            top = inlink;
        } else
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
            cur = cur.getNext();
        }
        return arry;
    }

    public Vector3 getTop()
    {
        return top.getData();
    }

    public Vector3 getBottom()
    {
        return bottom.getData();
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

   public  void setNext(TargLink nexto)
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