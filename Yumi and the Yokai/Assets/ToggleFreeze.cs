using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFreeze : MonoBehaviour
{
    //private GameObject stream;
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
                iceTimer = new timer(0, 0f);
            }
        }
    }
    void OnCollisionEnter(Collider hitInfo)
    {
        IceSpellObject iceSpell = hitInfo.GetComponent<IceSpellObject>();
        if (iceSpell != null)
        {
            this.GetComponent<Renderer>().material = ice;
            //turn the water into ice
        }
        //hitInfo.gameObject.tag == "Player" ||
        if ( hitInfo.gameObject.tag == "Human")
        {
            Debug.Log("We made it to here.");
            this.GetComponent<Renderer>().material = ice;
            waterBlockCollider.enabled = true;
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
