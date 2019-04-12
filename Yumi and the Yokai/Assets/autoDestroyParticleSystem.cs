/*autoDestroyParticleSystem.cs
********************************************************************************
*Creator(s)..........................................................Darrell Wong
*Created................................................................4/12/2019
*Last Modified..........................................................4/12/2019
*Last Modified by....................................................Darrell Wong
*
*   Description: This script is to be attached to the emitter object. 
*                   This script is necessary because when a particle emitter is deleted,
*                   all of the particles abruptly get deleted too.
*                   
*                   This should be attached to the particle emitter object. When 
*                   the object is detached from the parent projectle, it will delete
*                   the particle system after a delay.
********************************************************************************
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoDestroyParticleSystem : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.parent == null)
        {
            Destroy(gameObject, 2);
        }
    }
}
