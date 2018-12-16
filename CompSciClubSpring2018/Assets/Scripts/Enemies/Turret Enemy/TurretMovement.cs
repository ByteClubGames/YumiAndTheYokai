/*
***************************************************************************************
*Creator(s).........................................Keiran Glynn & Karim Dabboussi
*Created..............................................................3/17/2018
*Last Modified............................................@ 12:17 AM on 12/16/2018
*Last Modified by...................................................Karim Dabboussi
*
*Description:  controls the movement of the turrent enemy.
*
*               
***************************************************************************************
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMovement : MonoBehaviour {

    public GameObject point1;
    public GameObject point2;
    private Vector3 tempPosition;
    public float horizontalSpeed = 0.1f;
    public float progress = 0f;
    private Vector3 pos1;
    private Vector3 pos2;
  

    void Start()
    {
        tempPosition = transform.position;
        //default position
        float x = point1.transform.position.x;
        float y = point1.transform.position.y;
        float x2 = point2.transform.position.x;
        float y2 = point2.transform.position.y;
        pos1 = new Vector3(x, y, 0);
        pos2 = new Vector3(x2, y2, 0);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        progress = Mathf.PingPong(Time.time * horizontalSpeed, 1f);
        tempPosition.x = Mathf.Lerp(pos1.x, pos2.x, progress * 1);
        transform.position = Movement(tempPosition.x);
    }
    public Vector3 Movement(float x)
    {
        return new Vector3(x, transform.position.y, 0f);
    }
}