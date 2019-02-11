using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPillar : MonoBehaviour
{

    Material main_mat;

    Vector2 original_scale;

    // Use this for initialization
    void Start()
    {
        main_mat = GetComponent<MeshRenderer>().materials[0];
        original_scale = main_mat.mainTextureScale;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 0.5f + 0.5f * Mathf.Sin(Time.time));
        main_mat.mainTextureScale = new Vector2(original_scale.x, original_scale.y + gameObject.transform.localScale.z);
        Debug.Log(main_mat.mainTextureScale);
    }
}
