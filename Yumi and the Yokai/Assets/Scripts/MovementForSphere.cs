using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MovementForSphere : MonoBehaviour
{
    public Vector3 enemyPos;
    public Material normalSkin;
    public Material ice;
    public CapsuleCollider enemyCollider;
    timer iceTimer = new timer();
    int countDown = 5;
    int i = 0;
    //public float speed = 5;
    //public float updatedSpeed;
    //public bool dirRight;
    IceProjectile iceBall;
    EnemyPig pigmovement;
    //bool newSpeed;
    public Rigidbody pigBody;
    

    private void OnTriggerEnter(Collider colReg) // Will be replaced with ice spell usage, but for now this is to test
    {
        iceBall = colReg.GetComponent<IceProjectile>();
        pigmovement = colReg.GetComponent<EnemyPig>();
        pigBody = this.GetComponent<Rigidbody>();
        //newSpeed = pigmovement.enemyPatrols;
        if (iceBall != null) //turns the water into ice
        {
            pigmovement.enabled = false;
            pigBody.constraints = RigidbodyConstraints.FreezePositionX;
            //this.GetComponent<Renderer>().material = ice; // Changes the renderer of the block
            iceTimer = new timer(countDown, Time.time);
        }

    }
    
    // Start is called before the first frame update
    void Start()
    {
        enemyCollider = this.GetComponent<CapsuleCollider>();
        timer iceTimer = new timer(0, 0f);
    }

    void FixedUpdate()
    {

        if (iceTimer.waitTime != 0) 
        {
            if (iceTimer.checkTimer(Time.time))
            {
                iceTimer = new timer(0, 0f);// Resets the timer
                pigBody.constraints = RigidbodyConstraints.None;
                //this.GetComponent<Renderer>().material = normalSkin;// Changes the renderer of the "eneny"
                //newSpeed = true; ; // Resets the speed of the object
            }
        }
        /* Currently is used for testing purposes
        // Manages the movement of the "enemy"
        if (dirRight) {
            transform.Translate(Vector3.right * speed * Time.deltaTime); // Moves right
        }

        else { 
            transform.Translate(-Vector3.right * speed * Time.deltaTime); // Moves left
        }

        if (transform.position.x >= 4.0f) // Checks if the bound of movement is exceded
        {
            dirRight = false;  // When out of bounds, will move back within the bounds
        }

        if (transform.position.x <= -4)
        {
            dirRight = true; // When out of bounds, will move back within the bounds
        }
        */
    }

<<<<<<< Updated upstream
    void Update()
    {
        Vector3 v = startPos;
        v.x += delta * Mathf.Sin(movementTimer.ElapsedMilliseconds * speed / 1000);
        transform.position = v;
=======
    // A timer that manages time....
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
>>>>>>> Stashed changes
    }
}
