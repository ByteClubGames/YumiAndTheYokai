
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

    /*void Start()
    {
        this.GetComponent<TimeManager>().StartSlowDown(); // Time is slowed when spawner is here
    }
    */
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
            Vector3 p = Camera.main.ScreenToWorldPoint(new
               Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            Instantiate(iceSpellPrefab, new Vector3(p.x, p.y, 0.0f),
                        Quaternion.identity);
            iceSpellPrefab.transform.position = Input.mousePosition;
            //Runs ice prefab is mouse button is pushed down
        
        }

        /* 
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 playerinput = new Vector3(hit.point.x, hit.point.y, 0);
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