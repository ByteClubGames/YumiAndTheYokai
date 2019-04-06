/* 
 * Authors: Ivonne Lopez
 * Date Created:  04/06/2019 @  2:00 PM
 * Date Modified: 04/06/2019 @  3:00 PM
 * Project: YumiAndTheYokai
 * Description: This script enables/disables the renderer of any object passed to the visibility controller. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    public GameObject InvisibleObject;

    public void SetVisible()
    {
        //Handles UnassignedReferenceException
        try
        {
            InvisibleObject.GetComponent<Renderer>().enabled = true;
        }
        catch (UnassignedReferenceException)
        {
            Debug.Log("Missing Object");
        }
    }
    public void SetInvisible()
    {
        try {
            //disables renderer 
            InvisibleObject.GetComponent<Renderer>().enabled = false;
        }
        catch (UnassignedReferenceException)
        {
            Debug.Log("Missing Object");
        }
    }
}
