/********************************************************************************
*Creator(s)..........................................................Darrell Wong
*Created................................................................10/9/2018
*Last Modified................................................@ 7PM on 10/19/2018
*Last Modified by....................................................Darrell Wong
*
*  Description:    Script defines the rigidbody movement of the enemy pig to 
*                  patrol between 2 points(leftBoundry & rightBoundry) 
*                  and chasethe player
*                 
*                  Calls on PlayerDetection.cs to detect the player's presence.
*  
*  Instructions: 
*      1. Place enemy then set rightBoundry and leftBoundry. These boundries 
*          dictate how far to the left and right the enemy will patrol.
*      2. Adjust player detection collider size.
*      3. Max speeds and accelerations can be adjusted to taste.
*********************************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPig : MonoBehaviour {

    [Header("Physics/ Enemy Attributes")]
    public float AttackMaxSpeed;
    public float patrolMaxSpeed;
    public float acceleration;          //determines magnitude of force applied to enemy
    public float leftBoundry;           //sets the left patrol range  
    public float rightBoundry;          //sets the right patrol range
    private Vector3 leftPos;            //location of left boundry
    private Vector3 rightPos;           //location of right boundry
    public float idlePauseTime = 2;
    public float wallCheckRayLength = .5f;     //adjusting this value will determine how far away from walls the enemy will approach
    public float explosionRadius;
    public float chainExplodeDelay = .11f;
    private Vector3 originalPosition;
    private float startTimer;

    [Header("Boolean triggers")]
    public bool enemyPatrols = true;
    public bool hasIdlePause = true;
    private bool skidding;              //currently not being used
    private bool movingLeft;            //currently not being used
    private bool movingRight;           //currently not being used
    private bool idlePaused = false;    //when paused
    private bool challenged = false;    // Determines if the enemy should be attacking the player or not
    private bool patrolRight = true;
    public bool canExplode = true;
    public bool canChainExplode = true;
    private bool exploded = false;      //indicates whether this pig has already exploded

    [Header("Animation")]
    public GameObject pig_explosion;
    private Animator animator;
    private SpriteRenderer sprite_renderer;

    [Header("Objects")]
    private PlayerDetection detectPlayer;
    Rigidbody enemy;
    private GameObject tempVar;
    private Transform Player;

    void Start() {
        enemy = GetComponent<Rigidbody>();
        enemy.isKinematic = false;

        Player = GameObject.Find("Yumi").transform;
        detectPlayer = this.GetComponentInChildren<PlayerDetection>();

        originalPosition = transform.position;

        animator = this.GetComponentInChildren<Animator>();
        sprite_renderer = this.GetComponentInChildren<SpriteRenderer>();
    }

    void FixedUpdate() {


        leftPos = new Vector3(originalPosition.x - leftBoundry, 0f, 0f);    //set leftpos
        rightPos = new Vector3(originalPosition.x + rightBoundry, 0f, 0f);  //set rightpos

        Debug.DrawLine(new Vector3(rightPos.x, enemy.transform.position.y, 0f), new Vector3(leftPos.x, enemy.transform.position.y, 0f), Color.blue);

        Movement();
    }

    private void Update()
    {
        challenged = detectPlayer.PlayerDetected();             //update challenged boolean every frame
    }

    private void Movement()
    {
        AnimatorStateInfo anim_state = animator.GetCurrentAnimatorStateInfo(0);

        if (challenged)                                         //when the player is within range
        {
            if (anim_state.IsName("pig_idle"))
            {
                animator.Play("pig_wakeup");
            }
            GameObject Youkai = GameObject.Find("Player-Ferrox(Clone)");
            if (Youkai != null)
            {
                moveToPosition(Youkai.transform.position - transform.position, AttackMaxSpeed);
            }
            else
            {
                Vector3 directionOfPlayer = Player.transform.position - transform.position;
                moveToPosition(directionOfPlayer, AttackMaxSpeed);  //move towards the player
            }
        }
        else
        {
            if (idlePaused  || !enemyPatrols)
                animator.Play("pig_idle");
            else
                animator.Play("pig_run");

            if (enemyPatrols)
            {

                if (patrolRight)                               //This if-else block makes the enemy wander left to right based on left and right boundries
                {

                    Vector3 directionToNext = rightPos - enemy.transform.position;      //finds the direction vector to the right boundry
                    RaycastHit hit;
                    Debug.DrawLine(enemy.transform.position, enemy.transform.position + Vector3.right * wallCheckRayLength, Color.red);

                    if (directionToNext.x < 0 ||
                        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, wallCheckRayLength))  //else if the enemy has passed the right boundry
                    {
                        patrolRight = false;   //switch directions
                        idlePaused = true;

                    }
                    else                                                             //if the enemy is to the left of the boundry

                    {
                        if (!idlePaused)
                        {
                            moveToPosition(directionToNext, patrolMaxSpeed);        //move towards the boundry
                            startTimer = Time.time;
                        }
                        else
                        {
                            if (Time.time < (startTimer + idlePauseTime) && hasIdlePause)           //timer for pause time
                            {

                                enemy.AddForce(new Vector3(-enemy.velocity.x * (acceleration / 4f), 0f, 0f));       //when entering idle slow enemy to a stop
                            }
                            else
                            {
                                idlePaused = false;
                                moveToPosition(directionToNext, patrolMaxSpeed);    //move towards the boundry
                            }
                        }
                    }
                }
                else                                            //the enemy has now switched directions
                {
                    if (idlePaused)
                        animator.Play("pig_idle");
                    else
                        animator.Play("pig_run");

                    Vector3 directionToNext = leftPos - enemy.transform.position;
                    RaycastHit hit;
                    Debug.DrawLine(enemy.transform.position, enemy.transform.position + Vector3.left * wallCheckRayLength, Color.red);

                    if (directionToNext.x > 0 ||
                        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, wallCheckRayLength))
                    {
                        patrolRight = true;
                        idlePaused = true;
                    }
                    else
                    {
                        if (!idlePaused)
                        {
                            moveToPosition(directionToNext, patrolMaxSpeed);//move towards the boundry
                            startTimer = Time.time;
                        }
                        else
                        {
                            float currentTime = Time.time;
                            if (Time.time < (startTimer + idlePauseTime) && hasIdlePause)
                            {
                                enemy.AddForce(new Vector3(-enemy.velocity.x * (acceleration / 4f), 0f, 0f));   //when entering idle slow enemy to a stop
                            }
                            else
                            {
                                idlePaused = false;
                                moveToPosition(directionToNext, patrolMaxSpeed); //move towards the boundry
                            }
                        }
                    }
                }
            }
        }
    }

    private void moveToPosition(Vector3 targetDirection, float speed)  //applies force towards targetDirection (targetDirection = targetPos - currentPos)
    {
        if (targetDirection.x >= 0)                             //if targetDirection is to the right
        {
            if (enemy.velocity.x < speed)                       //if enemy speed is less than max speed 
            {
                enemy.AddForce(Vector3.right * acceleration);   //add force to the right
                sprite_renderer.flipX = false;
            }
        }
        else                                                    // if target direction is to the left
        {
            if (enemy.velocity.x > -speed)
            {
                enemy.AddForce(Vector3.left * acceleration);    //add force to the left
                sprite_renderer.flipX = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.name == "Yumi" || collision.gameObject.name == "Player-Ferrox(Clone)") && canExplode)
        {
            explode();
        }
    }

    private void explode()                          //called when triggered explosion by touching player
    {
        if (!exploded)                              //prevent method from being called again (prevents recursion)
        {
            exploded = true;
            Collider[] explosion = Physics.OverlapSphere(enemy.transform.position, explosionRadius);
            Instantiate(pig_explosion, this.transform.position + Vector3.down * 0.5f, Quaternion.identity);
            Destroy(this.gameObject);
            if (canChainExplode)
            {
                foreach (Collider inExplosion in explosion)
                {
                    if (inExplosion.gameObject.tag == "Enemy")
                    {
                        inExplosion.SendMessage("explodeOtherObjects", inExplosion, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
    }

    IEnumerator explodeOtherObjects()                       //called when explosions are chained together
    {
        yield return new WaitForSeconds(chainExplodeDelay);

        if (!exploded)                                      //prevent method from being called again (prevents recursion)
        {
            this.exploded = true;
            Collider[] explosion = Physics.OverlapSphere(enemy.transform.position, explosionRadius);
            Instantiate(pig_explosion, this.transform.position + Vector3.down * 0.5f, Quaternion.identity);
            Destroy(this.gameObject);
            foreach (Collider inExplosion in explosion)
            {
                if (inExplosion.gameObject.tag == "Enemy")
                {
                    inExplosion.SendMessage("explode", inExplosion, SendMessageOptions.DontRequireReceiver);  //calls explodeOtherObjects to continue chain explosion
                }
            }
        }
    }

    private void OnDrawGizmos()                     //see explosion radius
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
