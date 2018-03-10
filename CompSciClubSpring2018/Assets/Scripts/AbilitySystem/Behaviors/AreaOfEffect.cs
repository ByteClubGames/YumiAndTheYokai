/* AreaOfEffect.cs
 * Date Created: 3/7/18
 * Last Edited: 3/10/18
 * Programmer: Jack Bruce
 * Description: Behavior certain abilities will have. Makes ability only effective
 * for a certain surrounding area.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

[RequireComponent(typeof(SphereCollider))]
public class AreaOfEffect : AbilityBehaviors {

    private const string abName = "Area of Effect";
    private const string abDescription = "An area of damage!";
    private const BehaviorStartTimes startTime = BehaviorStartTimes.End; //on impact
    //private const Sprite icon = Resources.Load();

    private float areaRadius; //radius of sphere collider
    private float effectDuration; //how long the effect lasts
    private Stopwatch durationTimer = new Stopwatch();
    private float baseEffectDamage;
    private bool isOccupied;
    private float damageTickDuration;

    public AreaOfEffect(float ar, float ed, float bd)
        : base(new BasicObjectInformation(abName, abDescription), startTime)
    {
        areaRadius = ar;
        effectDuration = ed;
        baseEffectDamage = bd;
        isOccupied = false;
    }

    public override void PerformBehavior(GameObject playerObject, GameObject objectHit)
    {
        SphereCollider sc = this.gameObject.GetComponent<SphereCollider>();

        /*if (this.gameObject.GetComponent<SphereCollider>() == null)
            sc = this.gameObject.AddComponent<SphereCollider>();
        else
            sc = this.gameObject.GetComponent<SphereCollider>();*/
        
        sc.radius = areaRadius;
        sc.isTrigger = true;

        StartCoroutine(AOE());
    }

    private IEnumerator AOE() 
    {
        durationTimer.Start(); // turns on timer

        while (durationTimer.Elapsed.TotalSeconds <= effectDuration) // could be modified for ice spell since it probably isn't gonna do damage.
        {
            
            if(isOccupied)
            {
                //onDamage(list<targets>, baseDamage);
            }

            yield return new WaitForSeconds(damageTickDuration);

        }

        durationTimer.Stop();
        durationTimer.Reset();

        yield return null;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(isOccupied)
        {
            //do damage here
        }
        else
            isOccupied = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isOccupied = false;
    }
	
}
