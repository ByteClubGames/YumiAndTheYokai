using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : FlyingEnemy {
    public Transform Player;
    public float speed = 0.1f;
    private Vector3 directionOfPlayer;
    private bool challenged = false;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        //gets direction to move towards
        if (challenged)
        {
            directionOfPlayer = Player.transform.position - transform.position;
            directionOfPlayer = directionOfPlayer.normalized;
            transform.Translate(directionOfPlayer * speed, Space.World);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            challenged = true;
        }
    }
}
