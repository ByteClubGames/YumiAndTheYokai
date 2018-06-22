/* DamageOverTime.cs
 * Date Created: 3/10/18
 * Last Edited: 3/10/18
 * Programmer: Jack Bruce
 * Description: Behavior for inflicting damage over time
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DamageOverTime : AbilityBehaviors {

    private const string abName = "Damage Over Time";
    private const string abDescription = "An dot!";
    private const BehaviorStartTimes startTime = BehaviorStartTimes.Beginning; //on impact
    //private const Sprite icon = Resources.Load();

    private float effectDuration; //how long the effect lasts
    private Stopwatch durationTimer = new Stopwatch();
    private float baseEffectDamage;
    private float damageTickDuration;

    public DamageOverTime(float ed, float bd, float dtd)
        : base(new BasicObjectInformation(abName, abDescription), startTime)
    {
        effectDuration = ed;
        baseEffectDamage = bd;
        damageTickDuration = dtd;
    }

    public override void PerformBehavior(GameObject playerObject, GameObject objectHit)
    {
        StartCoroutine(DOT());
    }

    private IEnumerator DOT()
    {
        durationTimer.Start(); // turns on timer

        while (durationTimer.Elapsed.TotalSeconds <= effectDuration) // could be modified for ice spell since it probably isn't gonna do damage.
        {
            //onDamage(list<targets>, baseDamage);
            yield return new WaitForSeconds(damageTickDuration); 
        }

        durationTimer.Stop();
        durationTimer.Reset();

        yield return null;
    }

}
