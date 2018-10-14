using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityController : MonoBehaviour
{

    public GameObject InvisibleObject;

    public void SetVisible()
    {
        InvisibleObject.SetActive(true);
    }
    public void SetInvisible()
    {
        InvisibleObject.SetActive(false);
    }
}
