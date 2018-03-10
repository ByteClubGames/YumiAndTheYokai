using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Ability {

    private const string aName = "Fire Ball";
    private const string aDescription = "A firey mass that explodes on impact!";
    //private const Sprite icon = Resources.Load();

    private const float baseEffectDamageAOE = 50f;
    private const float baseEffectDamageDOT = 10f;

	//ranged, at the start, max distance, area of effect
    public FireBall() 
        : base(new BasicObjectInformation(aName, aDescription))
    {
        this.AbilityBehaviors.Add(new Ranged(10f, 20f, true));
        this.AbilityBehaviors.Add(new AreaOfEffect(2f, 2.3f, baseEffectDamageAOE));
        this.AbilityBehaviors.Add(new DamageOverTime(45f, baseEffectDamageDOT, 5f));
    }
}
