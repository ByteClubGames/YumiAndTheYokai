using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MovementForSphere : MonoBehaviour
{

    public float delta = 10.0f;  // Amount to move left and right from the start point
    public float speed = 2.0f;
    private Vector3 startPos;
    private Stopwatch movementTimer = new Stopwatch();

    void Start()
    {
        startPos = transform.position;
        movementTimer.Start();
    }

    void Update()
    {
        Vector3 v = startPos;
        v.x += delta * Mathf.Sin(movementTimer.ElapsedMilliseconds * speed / 1000);
        transform.position = v;
    }

}
