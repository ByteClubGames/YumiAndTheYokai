using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    public GameObject InvisibleObject;

    public void SetVisible()
    {
        
        try
        {
            InvisibleObject.SetActive(true);
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError(InvisibleObject);
        }
    }
    public void SetInvisible()
    {
        try {
            InvisibleObject.SetActive(false);
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError(InvisibleObject);
        }
    }
}
