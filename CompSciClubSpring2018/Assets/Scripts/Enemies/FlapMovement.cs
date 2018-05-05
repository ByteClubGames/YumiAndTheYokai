using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlapMovement : MonoBehaviour {
    private Vector2 currFlapPos;
    private Vector2 startFlapPos;
    private float flapRot;
    private Transform flapRB;
    private float minRange = 0f;
    private float maxRange = 0f;
    private int direction = -1;
    private int directionB = -1;

    public float patrolRange = 5f;
    public float speed = 2f;
    public float bounceHeight = 1f;

    // Use this for initialization
    void Start ()
    {
        flapRB = GameObject.Find("FlapEnemy").GetComponent<Transform>();
        startFlapPos = GameObject.Find("FLapEnemy").GetComponent<Transform>().position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        OnPatrol(patrolRange);
        Bounce();
    }

    private void OnPatrol(float range)
    {
        Debug.Log("FlapEnemy should be moving");
        currFlapPos = GameObject.Find("FlapEnemy").GetComponent<Transform>().position;
        flapRot = GameObject.Find("FlapEnemy").GetComponent<Transform>().eulerAngles.z;
        range = patrolRange;
        minRange = -1 * range;
        maxRange = range;
        if ((flapRot == 0) || (flapRot == 180))
        {
            switch (direction)
            {
                case -1:
                    if (currFlapPos.x > (startFlapPos.x + minRange))
                    {
                        flapRB.transform.Translate(-transform.right * Time.deltaTime * speed);
                    }
                    else
                    {
                        direction = 1;
                    }
                    break;
                case 1:
                    if (currFlapPos.x < (startFlapPos.x + maxRange))
                    {
                        flapRB.transform.Translate(transform.right * Time.deltaTime * speed);
                    }
                    else
                    {
                        direction = -1;
                    }
                    break;
            }
        }
        else if ((flapRot == 90) || (flapRot == 270))
        {
            switch (direction)
            {
                case -1:
                    if (currFlapPos.y > (startFlapPos.y + minRange))
                    {
                        flapRB.transform.Translate(transform.up * Time.deltaTime * speed);
                    }
                    else
                    {
                        direction = 1;
                    }
                    break;
                case 1:
                    if (currFlapPos.y < (startFlapPos.y + maxRange))
                    {
                        flapRB.transform.Translate(-transform.up * Time.deltaTime * speed);
                    }
                    else
                    {
                        direction = -1;
                    }
                    break;
            }
        }
        else
        {
            Debug.Log("Fix the rotation of FlapEnemy to be perpendicular to surface");
        }
    }

    private void Bounce()
    {
        Debug.Log("FlapEnemy should be bouncing");
        currFlapPos = GameObject.Find("FlapEnemy").GetComponent<Transform>().position;
        flapRot = GameObject.Find("FlapEnemy").GetComponent<Transform>().eulerAngles.z;        
        minRange = -1 * bounceHeight;
        maxRange = bounceHeight;

        switch (directionB)
        {
            case -1:
                if (currFlapPos.y > (startFlapPos.y + minRange))
                {
                    flapRB.transform.Translate(-transform.up * Time.deltaTime * speed);
                }
                else
                {
                    directionB = 1;
                }
                break;
            case 1:
                if (currFlapPos.y < (startFlapPos.y + maxRange))
                {
                    flapRB.transform.Translate(transform.up * Time.deltaTime * speed);
                }
                else
                {
                    directionB = -1;
                }
                break;
        }
    }
}
