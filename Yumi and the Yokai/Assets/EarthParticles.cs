using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EarthParticles : MonoBehaviour
{
    private Vector2 firstPressPos;
    private bool grow = false;
    //bool firstCollision;
    public int maxSpells = 3;
    public int destroyTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        //firstCollision = false;
        firstPressPos = Input.mousePosition;
        Destroy(transform.parent.gameObject, destroyTime); //Destroy timer starts on creation 
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            grow = true;
            #region LegacyCode
            //secondPressPos = Input.mousePosition;
            //calculate the difference between firstposition and second position
            //deltaX = secondPressPos.x - firstPressPos.x;
            //deltaY = secondPressPos.y - firstPressPos.y;
            //print("First: " + firstPressPos.x + " " + firstPressPos.y + "\n");
            //print("Second: " + secondPressPos.x + " " + secondPressPos.y + "\n");
            #endregion
        }

        //destroy the earliest spell when there are too many
        if (GameObject.FindGameObjectsWithTag("Earth Particle Object").Length > maxSpells)
        {
            Destroy(GameObject.FindGameObjectsWithTag("Earth Particle Object")[0]);
        }
    }
}