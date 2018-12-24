using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChildArraySorter : MonoBehaviour
{
    public Transform[] arr;

    void Start()
    {
        arr = GetComponentsInChildren<Transform>(true);
    }
}
