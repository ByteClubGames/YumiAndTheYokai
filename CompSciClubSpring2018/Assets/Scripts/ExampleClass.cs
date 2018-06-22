using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class ExampleClass : MonoBehaviour
{
    public GameObject particle;
    public Transform obj;

    private Touch toch; 
    private Vector2 objP; 

    void Update()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                // Create a particle if hit
                if (Physics.Raycast(ray))
                {
                    Instantiate(particle, transform.position, transform.rotation);
                }
            }
        }

        obj.position = toch.position; 

    }

}
