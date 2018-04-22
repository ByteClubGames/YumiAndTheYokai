/* Ranged.cs
 * Date Created: 3/7/18
 * Last Edited: 3/10/18
 * Programmer: Jack Bruce
 * Description: Behavior certain abilities will have. Makes ability only effective
 * a certain distance away. That is the lifedistance.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : AbilityBehaviors {

    private const string abName = "Ranged";
    private const string abDescription = "A ranged atttack!";
    private const BehaviorStartTimes startTime = BehaviorStartTimes.Beginning;
    //private const Sprite icon = Resources.Load();

    private float minDistance;
    private float maxDistance;

    private bool isRandomOn; // probably won't need this

    private float lifeDistance; 

    public Ranged(float minDist, float maxDist, bool isRandom) 
        : base(new BasicObjectInformation(abName, abDescription), startTime) 
    {
        minDistance = minDist;
        maxDistance = maxDist;
        isRandomOn = isRandom;
    }

    public override void PerformBehavior(GameObject playerObject, GameObject objectHit)
    {
        lifeDistance = isRandomOn ? Random.Range(minDistance, maxDistance) : maxDistance;

        StartCoroutine(CheckDistance(playerObject.transform.position));
    }

    private IEnumerator CheckDistance(Vector3 startPosition)
    {
        float tempdistance = Vector3.Distance(startPosition, this.transform.position);
        while (tempdistance < lifeDistance)
        {
            tempdistance = Vector3.Distance(startPosition, this.transform.position);
        }

        this.gameObject.SetActive(false); //object pooling code if we want or destroy
        yield return null;
    }

    public float MinDistance
    {
        get { return minDistance; }
    }

    public float MaxDistance
    {
        get { return maxDistance; }
    }

}
