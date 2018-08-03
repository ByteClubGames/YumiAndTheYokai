/**
 * Author(s): Brenda De los Santos
 * Date Created: 8/2/2018
 * Last Modified: 8/3/2018
 * File Name: FollowPath.cs
 * Description: This programs controls the motion of the platform. It allows the user
 * to select a follow type of MoveTowards(default) and Lerp. 
 * This program takes influence from the creators of 3DBuzz's video "Creating 2D 
 * Games in Unity 4.5 #4 - Moving Platforms"
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

	public enum FollowType
    {
        MoveTowards,
        Lerp
    }

    //Sets default FollowType to MoveTowards
    public FollowType Type = FollowType.MoveTowards;

    //Reference to a path the platform needs to follow
    public PathDefinition Path;

    public float speed = 1;

    //The maximum distance to a point in the path that is allowed until it move to 
    //the next point in the path
    public float MaxDistanceToGoal = 0.1f;

    //Creates a path enumerator, allows you to retrieve the current point in the path in the right order
    private IEnumerator<Transform> currentPoint;

    public void Start()
    {
        //Alerts the user that a path is not being referenced
        if (Path == null)
        {
            Debug.LogError("Path cannot be null", gameObject);
            return;
        }

        currentPoint = Path.GetPathEnumerator();
        currentPoint.MoveNext();

        //Breaks out of start method because there are no points in the path
        if (currentPoint.Current == null)
            return;
        
        //Sets the position of the object to the first point in the path
        transform.position = currentPoint.Current.position;

    }


    public void Update()
    {
        //Breaks out of update if the object was created without a path or was created with a path that didn't atleast have one point
        if (currentPoint == null || currentPoint.Current == null)
            return;

        //Moves the platform according to the FollowType selected
        if (Type == FollowType.MoveTowards)
            transform.position = Vector3.MoveTowards(transform.position, currentPoint.Current.position, speed * Time.deltaTime);
        else if (Type == FollowType.Lerp)
            transform.position = Vector3.Lerp(transform.position, currentPoint.Current.position, speed * Time.deltaTime);

        //Determines whether the platform is close enough to the target point to begin moving to the next point
        var distanceSquared = (transform.position - currentPoint.Current.position).sqrMagnitude;
        if (distanceSquared < MaxDistanceToGoal * MaxDistanceToGoal)
            currentPoint.MoveNext();
    }
}
