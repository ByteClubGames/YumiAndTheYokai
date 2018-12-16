/*
********************************************************************************
*Creator(s)........................................................Hunter Goodin
*Created..............................................................12/15/2018
*Last Modified...........................................@12:10 AM on 12/05/2018
*Last Modified by..................................................Hunter Goodin
*
*Description: This script handle's the Yokai's health. Every second, the health 
*             will decrement. The health will also go down based on any 
*             potential values passed to DamageYokai(). When the health gets to 
*             zero, the yokai will despawn. 
********************************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YokaiHealthSystem : MonoBehaviour
{
    public int health = 60;
    public float initialTime; 
    public float deltaTime;
    public float finalTime; 
    public float second = 1.0f;
    private VisibilityController invisibleObjects;
    private YokaiSwitcher switcher;

    void Start()
    {
        switcher = GameObject.Find("Yumi").GetComponentInChildren<YokaiSwitcher>();
        invisibleObjects = GameObject.Find("SceneDirector").GetComponentInChildren<VisibilityController>();
        SetTime(); 
    }

    void FixedUpdate()
    {
        if (health > 0)
        {
            if (Time.time > initialTime)
            {
                initialTime++; 
                health--; 
            }
        }

        if (health <= 0)
        {
            second = 1.0f;
            health = 60;
            SetTime();
            switcher.DeleteYokai(GameObject.Find("Yokai(Clone)"));
            invisibleObjects.SetInvisible();
            GameObject.Find("InputListener").GetComponent<InputListener>().SetYumiActive(true);
        } 
    }

    public void DamageYokai(int dam)
    {
        health = health - dam; 
    }

    public void SetTime()
    {
        initialTime = Time.time;
        finalTime = initialTime + health;
    }
}
