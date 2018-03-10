/* AbilityBehaviors.cs
 * Date Created: 3/7/18
 * Last Edited: 3/10/18
 * Programmer: Jack Bruce
 * Description: Parent class for all behaviors abilities have.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBehaviors : MonoBehaviour {

    private BasicObjectInformation objectInfo;
    private BehaviorStartTimes startTime;
    private BasicObjectInformation basicObjectInformation;

    public AbilityBehaviors(BehaviorStartTimes sTime) 
    {
        startTime = sTime;
    }

    public AbilityBehaviors(BasicObjectInformation basicObjectInformation, BehaviorStartTimes startTime)
    {
        this.basicObjectInformation = basicObjectInformation;
        this.startTime = startTime;
    }

    public enum BehaviorStartTimes 
    {
        Beginning,
        Middle,
        End
    }

    // we want a gameobject, our target,
    public virtual void PerformBehavior(GameObject playerObject, GameObject objectHit) 
    {
        Debug.LogWarning("NEED TO ADD BEHAVIOR");
    }

    public BasicObjectInformation AbilityBehaviorInfo
    {
        get { return objectInfo; }
    }

    public BehaviorStartTimes AbilityBehaviorStartTime
    {
        get { return startTime; }
    }

}
