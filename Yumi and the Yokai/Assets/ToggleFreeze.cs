/*
********************************************************************************
*Creator(s)......................................................Brenden Plong
*Created..............................................................1/26/2019
*Last Modified...........................................@ 11:42AM on 3/3/2019
*Last Modified by................................................Brenden Plong
*
*Description:   Handles the freezing of water so that the player can walk on 
*               it
********************************************************************************
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFreeze : MonoBehaviour
{
    public GameObject sphere;
    public Material ice;
    public Material water;
    public BoxCollider waterBlockCollider;
    int countDown = 5;
    int i = 0;
    timer iceTimer = new timer();
    // Start is called before the first frame update
    void Start()
    {
        waterBlockCollider = this.GetComponent<BoxCollider>();
        timer iceTimer = new timer(0, 0f);
        waterBlockCollider.enabled = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (iceTimer.waitTime != 0)
        {
            if (iceTimer.checkTimer(Time.time))
            {
                this.GetComponent<Renderer>().material = water;
                iceTimer = new timer(0, 0f); // Resets the timer for turning the ice back to water
                waterBlockCollider.enabled = false; // Disables the box collider at the start
            }
        }
    }
    void OnTriggerEnter(Collider hitInfo)
    {
        IceProjectile iceBall = hitInfo.GetComponent<IceProjectile>();
        if (iceBall != null)//turns the water into ice
        {
            Debug.Log("*Freezing*");
            this.GetComponent<Renderer>().material = ice; // Changes the renderer of the block
            waterBlockCollider.enabled = true; // Makes the box collider active
            iceTimer = new timer(countDown, Time.time);
        }
    }
    struct timer
    {
        float endTime;
        public int waitTime;
        public float curTime;
        public timer(int wait_Time, float cur_Time)
        {
            waitTime = wait_Time;
            curTime = cur_Time;
            endTime = waitTime + cur_Time;
        }
        public bool checkTimer(float curTime)
        {
            if(curTime > endTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
