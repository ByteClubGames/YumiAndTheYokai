/* Slow.cs
 * Date Created: 3/10/18
 * Last Edited: 3/10/18
 * Programmer: Jack Bruce
 * Description: Behavior certain abilities will have. Makes moving objects in
 * ability collider move slower.
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Slow : AbilityBehaviors {

    private const string abName = "Slow";
    private const string abDescription = "Slows object's moving speed!";
    private const BehaviorStartTimes startTime = BehaviorStartTimes.End; //on impact
    //private const Sprite icon = Resources.Load();

    private float effectDuration; //how long the effect lasts
    private float slowPercent;

    public Slow(float ed, float sp)
        : base(new BasicObjectInformation(abName, abDescription), startTime)
    {
        effectDuration = ed;
        slowPercent = sp;
    }

    public override void PerformBehavior(GameObject playerObject, GameObject objectHit)
    {
        StartCoroutine(SlowMovement(objectHit));
    }

    private IEnumerator SlowMovement(GameObject objectHit) //I HAVE NO IDEA IF THIS WILL WORK
    {
        Vector3 ogVel = objectHit.transform.GetComponent<Rigidbody>().velocity; //original velocity of enemy
        Vector3 objVel = objectHit.transform.GetComponent<Rigidbody>().velocity;

        while (objVel.magnitude > Vector3.zero.magnitude) //object is moving
        {
            objectHit.transform.GetComponent<Rigidbody>().velocity = objVel * slowPercent * .01f; //slows objectHit velocity
            objVel = objectHit.transform.GetComponent<Rigidbody>().velocity; // assigns current velocity to objVel
            yield return new WaitForSeconds(.5f); //apply this slowing down every .5 secs
        }

        yield return new WaitForSeconds(effectDuration); //wait for objHit to unfreeze

        //reset obj's movement speed
        if (objVel.magnitude < ogVel.magnitude)
        {
            objectHit.transform.GetComponent<Rigidbody>().velocity = ogVel;
        }

        yield return null;
    }

}
