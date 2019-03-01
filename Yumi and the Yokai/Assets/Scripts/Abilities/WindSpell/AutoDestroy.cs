/*AutoDestroy.cs
********************************************************************************
*Creator(s).........................................................Darrell Wong
*Created................................................................3/1/2019
*Last Modified..........................................................3/1/2019
*Last Modified by...................................................Darrell Wong
*
*Attatch to WindSpellImpact child object of Windspell & windspell agent 
* 
*   Description: Automatically destroys the windspell impact object that carries
*   impact animation
********************************************************************************
*/

using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour
{
    public float delay = 0f;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
    }
}