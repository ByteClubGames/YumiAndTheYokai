<<<<<<< Updated upstream
﻿/* IceSpellUse.cs
 * Date Created: 3/10/18
 * Last Edited: 6/16/18
 * Programmer: Jack Bruce
 * Description: Script for instantiating ice spell objects upon mouse clicks
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class IceSpellUse : MonoBehaviour {

    public GameObject iceSpellPrefab;
    private GameObject myCurrentObject;
    //variables for controlling spawnrate of icespells
    public float waitToSpawn;
    Stopwatch spawnTimer = new Stopwatch();


    private bool _isDragging = false;

    void Start()
    {
        this.GetComponent<TimeManager>().StartSlowDown(); // Time is slowed when spawner is here
        spawnTimer.Start();
    }

    void Update()
	{
		//Instantiates iceSpellPrefab upon clicking in the position of the click
        /*if (Input.GetMouseButtonDown(0))
        {
           
            Vector3 p = Camera.main.ScreenToWorldPoint(new 
                Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            Instantiate(iceSpellPrefab, new Vector3(p.x, p.y, 0.0f), 
                        Quaternion.identity);
           iceSpellPrefab.transform.position = Input.mousePosition;
        }*/

       if (Input.GetMouseButtonDown(0))
       { 
            _isDragging = true;
       }
       if (Input.GetMouseButtonUp(0))
       {
            _isDragging = false;
            return;
       }
       if (_isDragging)
       {
            if (spawnTimer.ElapsedMilliseconds < waitToSpawn * 1000) return;
            //Runs ice prefab is mouse button is pushed down
            Vector3 p = Camera.main.ScreenToWorldPoint(new
              Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            // needs to Wait for some time in between spawning. too many spells at one time

            Instantiate(iceSpellPrefab, new Vector3(p.x, p.y, 0.0f),
                        Quaternion.identity);
            iceSpellPrefab.transform.position = Input.mousePosition;
            spawnTimer.Reset();
            spawnTimer.Start();
       }


    }




    /*function Update()
    var objectToInstantiate : GameObject;
 private var myCurrentObject : GameObject;
 
 function Update(){
 if(Input.GetMouseButtonDown(0)){
 myCurrentObject = Instantiate(objectToInstantiate,Input.mousePosition,Quaternion.identity);
     }
 if(Input.GetMouseButton(0) && myCurrentObject){
 myCurrentObject.transform.position = Input.mousePosition;
     }
 if(Input.GetMouseButtonUp(0) && myCurrentObject){
 myCurrentObject = null;
     }
 
    }*/
}
=======
﻿/* IceSpellUse.cs
 * Date Created: 3/10/18
 * Last Edited: 9/13/18
 * Programmer: Jack Bruce, Brenden Plong
 * Description: Script for instantiating ice spell objects upon mouse clicks
 * Update @ 9/13/2018: Changed previous method of casting the spell to ray casting to allow for more "accurate" spawning.
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class IceSpellUse : MonoBehaviour
{

    public GameObject iceSpellPrefab;
    private GameObject myCurrentObject;
    private bool _isDragging = false;

    void Start()
    {
        this.GetComponent<TimeManager>().StartSlowDown(); // Time is slowed when spawner is here
    }

    void Update()
    {
        //Instantiates iceSpellPrefab upon clicking in the position of the click
        /*if (Input.GetMouseButtonDown(0))
        {
           
            Vector3 p = Camera.main.ScreenToWorldPoint(new 
                Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            Instantiate(iceSpellPrefab, new Vector3(p.x, p.y, 0.0f), 
                        Quaternion.identity);
           iceSpellPrefab.transform.position = Input.mousePosition;
        }*/

        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            return;
        }
        if (_isDragging) // Casts spell only if the mouse is held down and is dragging across the screen
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Casts a ray directly from the camera based on where the mouse's clicks are
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 playerinput = new Vector3(hit.point.x, hit.point.y, 0); // Takes the rays's collision
                Instantiate(iceSpellPrefab, new Vector3(hit.point.x, hit.point.y, 0.0f),
                            Quaternion.identity);
                iceSpellPrefab.transform.position = Input.mousePosition;

            }
            //Runs ice prefab is mouse button is pushed down
            /*Vector3 p = Camera.main.ScreenToWorldPoint(new
              Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            Instantiate(iceSpellPrefab, new Vector3(p.x, p.y, 0.0f),
                        Quaternion.identity);
            iceSpellPrefab.transform.position = Input.mousePosition;*/
        }

        /* 
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Instantiate(iceSpellPrefab, new Vector3(p.x, p.y, 0.0f),
                        Quaternion.identity);
            iceSpellPrefab.transform.position = Input.mousePosition;

        }
        /*




    /*function Update()
    var objectToInstantiate : GameObject;
 private var myCurrentObject : GameObject;
 
 function Update(){
 if(Input.GetMouseButtonDown(0)){
 myCurrentObject = Instantiate(objectToInstantiate,Input.mousePosition,Quaternion.identity);
     }
 if(Input.GetMouseButton(0) && myCurrentObject){
 myCurrentObject.transform.position = Input.mousePosition;
     }
 if(Input.GetMouseButtonUp(0) && myCurrentObject){
 myCurrentObject = null;
     }
 
    }*/
    }
}
>>>>>>> Stashed changes
