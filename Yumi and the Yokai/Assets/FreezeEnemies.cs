/*
********************************************************************************
*Creator(s)......................................................Brenden Plong
*Created..............................................................3/14/2019
*Last Modified...........................................@ 4:34PM on 3/14/2019
*Last Modified by................................................Brenden Plong
*
*Description:   Handles the freezing of enemies
*               
********************************************************************************
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEnemies : MonoBehaviour
{
    int countDown = 5;
    int i = 0;
    timer iceTimer = new timer();
    Rigidbody pigbody;
    EnemyPig pigmovement;

    // Start is called before the first frame update
    void Start()
    {
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
                pigmovement.enabled = true;
                pigbody.constraints = RigidbodyConstraints.FreezePositionZ;
            }
        }
    }

    void OnTriggerEnter(Collider hitInfo)
    {
        IceProjectile iceBall = hitInfo.GetComponent<IceProjectile>();
        pigbody = GetComponent<Rigidbody>();
        pigmovement = GetComponent<EnemyPig>();
        if (iceBall != null)//turns the water into ice
        {
            Debug.Log("*Freezing*");
            pigmovement.enabled = false;
            pigbody.constraints = RigidbodyConstraints.FreezeAll;
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
}
