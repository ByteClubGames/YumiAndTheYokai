/*
********************************************************************************
*Creator(s)......................................................Brenden Plong
*Created..............................................................1/26/2019
*Last Modified...........................................@ 11:42AM on 3/3/2019
*Last Modified by................................................Brenden Plong
*
*Description:   Handles the freezing of water so that the player can walk on 
*               it when frozen
****************************************************************************
* 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeCheck : MonoBehaviour
{
    public Material water;
    public Material ice;
    public BoxCollider waterBlockColliderCheck;
    timer iceTimer = new timer();
    int countDown = 5;
    int i = 0;

    private void OnTriggerEnter(Collider colReg) // Will be replaced with ice spell usage, but for now this is to test
    {
        if (colReg.gameObject.tag == "Player")
        {
            Debug.Log("Looks like water to me");
            waterBlockColliderCheck.enabled = false;

            iceTimer = new timer(countDown, Time.time); // Creates new timer
            //turn the water into ice
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
            if (curTime > endTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        waterBlockColliderCheck = this.GetComponent<BoxCollider>();
        waterBlockColliderCheck.enabled = true;
        timer iceTimer = new timer(0, 0f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (iceTimer.waitTime != 0)
        {
            if (iceTimer.checkTimer(Time.time))
            {
                iceTimer = new timer(0, 0f);
                waterBlockColliderCheck.enabled = true;
            }
        }
    }
}
