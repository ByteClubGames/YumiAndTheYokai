using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour {

    public float horizontalSpeed;
    public float verticalSpeed;
    public float amplitude;
    public Vector3 tempPosition;
    public float progress = 0f;
    public Transform Player;
    public float attackSpeed = 0.1f;
    private Vector3 directionOfPlayer;
    private bool challenged = false;
    private Vector3 pos1 = new Vector3(-4f, 5f, 0f);
    private Vector3 pos2 = new Vector3(4f, 5f, 0f);

	// Use this for initialization
	void Start () {
        //default position
        tempPosition = transform.position;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        progress = Mathf.PingPong(Time.time * 0.1f, 1f);
        tempPosition.x = Mathf.Lerp(pos1.x, pos2.x, progress * 1);
        tempPosition.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude;

        transform.position = Movement(tempPosition.x, tempPosition.y); 
	} 

public Vector3 Movement(float x, float y)
    {
        return new Vector3(x, y, 0f);
    }
    void Update()
    {
        //gets direction to move towards
        if (challenged)
        {
            directionOfPlayer = Player.transform.position - transform.position;
            directionOfPlayer = directionOfPlayer.normalized;
            transform.Translate(directionOfPlayer * attackSpeed, Space.World);
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
