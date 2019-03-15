/*
********************************************************************************
*Creator(s)......................................................Brenden Plong
*Created..............................................................1/26/2019
*Last Modified...........................................@ 11:42AM on 3/3/2019
*Last Modified by................................................Brenden Plong
*
*Description:   Handles the freezing of water so that the player can walk on 
*               it.
********************************************************************************
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFreeze : MonoBehaviour
{
    public Material ice;
    public Material water;
    public BoxCollider waterBlockCollider;
    public int countDown = 5; // Ammount of time (in seconds) before the ice thaws.
    Timer iceTimer = new Timer();

    void Start()
    {
        waterBlockCollider = this.GetComponent<BoxCollider>();
        Timer iceTimer = new Timer(0, 0f); // Initializing a "Null" timer.
        waterBlockCollider.enabled = false; // Starting the water in a "thawed" state.
    }
    
    /* Every frame, if there is an active timer, check if the required time has passed so that you can unfreeze the 
     * ice block. When the timer is up, we change the material back to a water material, create a null timer, and then
     * disable the collider on the water block so the player can not walk on it. */
    void FixedUpdate()
    {
        if (iceTimer.waitTime != 0) // Is there an active, valid, non-NULL timer?
        {
            if (iceTimer.checkTimer(Time.time))
            {
                this.GetComponent<Renderer>().material = water;
                iceTimer = new Timer(0, 0f); // Resets the timer for turning the ice back to water
                waterBlockCollider.enabled = false; // Disables the box collider at the start
            }
        }
    }

    /* When this function is entered, we check if the trigger that entered the collider was an ice spell projectile. If
     * it was, we change the material of the water to an ice material, enable the block's collider so it can be walked
     * on (freeze the water), and start a new timer that, when finished, will thaw the ice. */
    void OnTriggerEnter(Collider hitInfo)
    {
        IceProjectile iceBall = hitInfo.GetComponent<IceProjectile>();
        if (iceBall != null)
        { 
        Debug.Log("*Freezing*");
        this.GetComponent<Renderer>().material = ice;
        waterBlockCollider.enabled = true;
        iceTimer = new Timer(countDown, Time.time);
        }
    }

    /// <summary>
    /// This struct is essentially a timer object.
    /// </summary>
    struct Timer
    {
        float endTime;
        public int waitTime;
        public float curTime;

        /// <summary>
        /// This is the constructor for the Timer struct. It takes in the current time and the time you want it to wait for. It
        /// contains a method for checking if the specified wait time has elapsed or not.
        /// </summary>
        /// <param name="wait_Time">The time, in seconds, you would like the time to wait for.</param>
        /// <param name="cur_Time">This is the current time. You should probably pass in Time.time.</param>
        public Timer(int wait_Time, float cur_Time)
        {
            waitTime = wait_Time;
            curTime = cur_Time;
            endTime = waitTime + cur_Time;
        }

        /// <summary>
        /// Method that will return true if the the specified time to the timer has been passed.
        /// </summary>
        /// <param name="curTime">This is the current time. You should probably pass in Time.time.</param>
        /// <returns></returns>
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
