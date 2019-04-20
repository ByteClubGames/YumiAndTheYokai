using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStates
{
    /// <summary>
    /// Modifies player velocity so that they move as if they have jumped.
    /// </summary>
    /// <param name="velocity">Player's movement velocity for this frame.</param>
    /// <param name="jump_speed">Player jump speed.</param>
    /// <param name="gravity">World gravity.</param>
    public void Jump(ref Vector3 velocity, float jump_speed, float gravity)
    {
        velocity.y = Mathf.Sqrt(2f * jump_speed * -gravity);
    }

    /// <summary>
    /// Will half the current upward velocty this frame to lower height of jump.
    /// </summary>
    /// <param name="velocity">Player's movement velocity for this frame.</param>
    /// <param name="short_hop_coefficient">Factor by which to decrease jump height.</param>
    public void ShortHop(ref Vector3 velocity, float short_hop_coefficient)
    {
        velocity.y = velocity.y / short_hop_coefficient;
    }

    //public Vector3 Run()
    //{

    //}

    public void ApplyGravity(ref Vector3 velocity, float gravity)
    {
        velocity.y += gravity * Time.deltaTime;
    }

    //public Vector3 Idle()
    //{

    //}

    //public Vector3 TakeDamage()
    //{

    //}

    //public Vector3 Land()
    //{

    //}


}
