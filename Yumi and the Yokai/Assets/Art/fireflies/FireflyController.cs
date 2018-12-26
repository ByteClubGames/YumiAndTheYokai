using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localPosition += transform.forward * Time.deltaTime * 2f;
        this.transform.localPosition += transform.up * .1f * Mathf.Sin(Time.fixedTime * 3.0f * Random.Range(0.1f,0.5f));
    }
}
