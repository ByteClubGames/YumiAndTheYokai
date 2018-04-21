/* BackAndForth.cs
 * Date Created: 3/10/18
 * Last Edited: 3/17/18
 * Programmer: Jack Bruce
 * Description: Makes object oscilate. using for test enemy
 */

using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class BackAndForth : MonoBehaviour {

    public float delta = 55.0f;  // Amount to move left and right from the start point
    public float speed = 2.0f;
    private Vector3 startPos;
    public bool isFrozen = false;
    private bool lastFrameFroze = false;
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

        // When isFrozen is set to false (by IceSpell) pause movement
        if (isFrozen)
        {
            movementTimer.Stop();
            lastFrameFroze = true; //track if object was previosly frozen
        }

        // if object was frozen and it is not anymore resume movement
        if (lastFrameFroze && !isFrozen)
        {
            movementTimer.Start();
            lastFrameFroze = false;
        }
    }

}
