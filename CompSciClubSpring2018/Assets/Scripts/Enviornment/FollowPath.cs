/**
 * Author(s): Brenda De los Santos
 * Date Created: 8/2/2018
 * Last Modified: 8/2/2018
 * File Name: FollowPath.cs
 * Description: 
 * **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

	public enum FollowType
    {
        MoveTowards,
        Lerp
    }

    public FollowType Type = FollowType.MoveTowards;

    public PathDefinition Path;

    public float speed = 1;

    public float MaxDistanceToGoal = 0.1f;

    public IEnumerator<Transform> currentPoint;

    public void Start()
    {
        if (Path == null)
        {
            Debug.LogError("Path cannot be null", gameObject);
            return;
        }

        currentPoint = Path.GetPathEnumerator();
        currentPoint.MoveNext();

        if (currentPoint.Current == null)
            return;

        transform.position = currentPoint.Current.position;

    }


    public void Update()
    {
        if (currentPoint == null || currentPoint.Current == null)
            return;

        if (Type == FollowType.MoveTowards)
            transform.position = Vector3.MoveTowards(transform.position, currentPoint.Current.position, speed * Time.deltaTime);
        else if (Type == FollowType.Lerp)
            transform.position = Vector3.Lerp(transform.position, currentPoint.Current.position, speed * Time.deltaTime);

        var distanceSquared = (transform.position - currentPoint.Current.position).sqrMagnitude;
        if (distanceSquared < MaxDistanceToGoal * MaxDistanceToGoal)
            currentPoint.MoveNext();
    }
}
