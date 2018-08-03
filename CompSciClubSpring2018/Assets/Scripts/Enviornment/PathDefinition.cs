/**
 * Author(s): Brenda De los Santos
 * Date Created: 8/2/18
 * Last Modified: 8/3/18
 * File Name: PathDefinition.cs
 * Description: This program hold an array of Transforms/points that we want the 
 * platform to traverse.
 * This program takes influence from the creators of 3DBuzz's video "Creating 2D 
 * Games in Unity 4.5 #4 - Moving Platforms"
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathDefinition : MonoBehaviour {

    [SerializeField]
    public Transform[] points;

    //Custom enumerator that will keep the platform moving from point to point (ping-pong motion)
    public IEnumerator<Transform> GetPathEnumerator()
    {
        //Will terminate the sequence immediately if there is nothing in the points array
        //Need at least two points to create a path
        if (points == null || points.Length < 1)
            yield break;

        var direction = 1; 
        var index = 0;

        //Iterator block, used to change direction 
        while (true)
        {
            yield return points[index];

            if (points.Length == 1)
                continue;

            if (index <= 0)
                direction = 1;

            else if (index >= points.Length - 1)
                direction = -1;

            index = index + direction;
        }
    }

    //Draws the path between points in the scene view of unity
    public void OnDrawGizmos()
    {
        //Checks if ther are atleast two points in the path
        if (points == null || points.Length < 2)
            return;

        //Draws the line fron the previous point to the current point
        for (var i = 1; i < points.Length; i++)
        {
            Gizmos.DrawLine(points[i - 1].position, points[i].position);
        }
    }
}
