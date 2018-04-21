using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMovement : MonoBehaviour {
    private Vector2 turretPos;
    private Transform turretRB;
    private float minRange = 0f;
    private float maxRange = 0f;
    private int direction = -1;

    public float patrolRange = 5f;
    public float speed = 2f;


	// Use this for initialization
	void Start () {
		turretRB = GameObject.Find("TurretEnemy").GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        OnPatrol(patrolRange);
	}

    private void OnPatrol(float range)
    {
        Debug.Log("I should be moving");
        turretPos = GameObject.Find("TurretEnemy").GetComponent<Transform>().position;
        range = patrolRange;
        minRange = -1 * range;
        maxRange = range;

        switch (direction)
        {
            case -1:
                if(turretPos.x > minRange)
                {
                    turretRB.transform.Translate(-transform.right * Time.deltaTime * speed);
                }
                else
                {
                    direction = 1;
                }
                break;
            case 1:
                if (turretPos.x < maxRange)
                {
                    turretRB.transform.Translate(transform.right * Time.deltaTime * speed);
                }
                else
                {
                    direction = -1;
                }
                break;
        }
    }
}
