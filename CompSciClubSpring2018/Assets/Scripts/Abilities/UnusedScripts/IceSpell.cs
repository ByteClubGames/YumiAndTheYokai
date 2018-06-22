/* IceSpell.cs
 * Date Created: 3/7/18
 * Last Edited: 3/10/18
 * Programmer: Jack Bruce
 * Description: Ice Spell will slow enemies and turn water tiles into ice.
 * most of those mechanics can probably be implemented in Slow behavior
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpell : Ability{

    private const string aName = "Ice Spell";
    private const string aDescription = "Freeze water and your friends!";
    //private const Sprite icon = Resources.Load();

    private const float slowEffectDuration = 5f; // how long will object be frozen
    private const float slowPercent = 50f; //objHit velocity will decrease by this percent every .5 secs
    private const float baseDmgAOE = 0f; //as of right now Ice does not cause damsge

    //ranged, at the start, max distance, area of effect
    public IceSpell()
        : base(new BasicObjectInformation(aName, aDescription))
    {
        //this will probably be a ranged effect... but will do that later
        this.AbilityBehaviors.Add(new AreaOfEffect(2f, 2.3f, baseDmgAOE));
        this.AbilityBehaviors.Add(new Slow(slowEffectDuration, slowPercent));
    }
	
}
