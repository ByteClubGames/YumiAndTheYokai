/* BackAndForth.cs
 * Date Created: 3/10/18
 * Last Edited: 3/10/18
 * Programmer: Jack Bruce
 * Description: Makes object oscilate. using for test enemy
 */

using UnityEngine;
using System.Collections;

public class BackAndForth : MonoBehaviour
{

    public float delta = 55.0f;  // Amount to move left and right from the start point
    public float speed = 2.0f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 v = startPos;
        v.x += delta * Mathf.Sin(Time.time * speed);
        transform.position = v;
    }
}
