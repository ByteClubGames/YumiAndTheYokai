using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlapMovement : MonoBehaviour {
    private Vector2 currFlapPos;
    private Vector2 startFlapPos;
    private Vector2 aceFlapPos;
    private Vector2 target; 
    private float flapRot;
    private Transform flapRB;
    private Rigidbody2D playerRB;
    private Rigidbody2D turretProjectileRB;
    private float minRange = 0f;
    private float maxRange = 0f;
    private int direction = -1;
    private int directionB = -1;
    private int directionC = -1;
    private bool overPlayer = false;
    private bool canAttack = true;

    public float patrolRange = 5f;
    public float patrolSpeed = 2f;
    public float bounceSpeed = 2f;
    public float bounceHeight = 1f;
    public float dropRange = 5f;
    public float projectileSpeed;

    // Use this for initialization
    void Start()
    {
        flapRB = GameObject.Find("FlapEnemy").GetComponent<Transform>();
        startFlapPos = GameObject.Find("FlapEnemy").GetComponent<Transform>().position;
        turretProjectileRB = this.GetComponent<Rigidbody2D>();
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (overPlayer)
        {
            Debug.Log("Bingo");
            Attack();
            ProjectileMovement();
        }
        else
        {
            OnPatrol(patrolRange);
            Bounce();
        }
    }

    private void OnPatrol(float range)
    {
        Debug.Log("FlapEnemy should be moving");
        currFlapPos = GameObject.Find("FlapEnemy").GetComponent<Transform>().position;
        flapRot = GameObject.Find("FlapEnemy").GetComponent<Transform>().eulerAngles.z;
        range = patrolRange;
        minRange = -1 * range;
        maxRange = range;
        if ((flapRot == 0) || (flapRot == 180))
        {
            switch (direction)
            {
                case -1:
                    if (currFlapPos.x > (startFlapPos.x + minRange))
                    {
                        flapRB.transform.Translate(-transform.right * Time.deltaTime * patrolSpeed);
                    }
                    else
                    {
                        direction = 1;
                    }
                    break;
                case 1:
                    if (currFlapPos.x < (startFlapPos.x + maxRange))
                    {
                        flapRB.transform.Translate(transform.right * Time.deltaTime * patrolSpeed);
                    }
                    else
                    {
                        direction = -1;
                    }
                    break;
            }
        }
        else if ((flapRot == 90) || (flapRot == 270))
        {
            switch (direction)
            {
                case -1:
                    if (currFlapPos.y > (startFlapPos.y + minRange))
                    {
                        flapRB.transform.Translate(transform.up * Time.deltaTime * patrolSpeed);
                    }
                    else
                    {
                        direction = 1;
                    }
                    break;
                case 1:
                    if (currFlapPos.y < (startFlapPos.y + maxRange))
                    {
                        flapRB.transform.Translate(-transform.up * Time.deltaTime * patrolSpeed);
                    }
                    else
                    {
                        direction = -1;
                    }
                    break;
            }
        }
        else
        {
            Debug.Log("Fix the rotation of FlapEnemy to be perpendicular to surface");
        }
    }

    private void Bounce()
    {
        Debug.Log("FlapEnemy should be bouncing");
        currFlapPos = GameObject.Find("FlapEnemy").GetComponent<Transform>().position;
        flapRot = GameObject.Find("FlapEnemy").GetComponent<Transform>().eulerAngles.z;
        minRange = -1 * bounceHeight;
        maxRange = bounceHeight;

        switch (directionB)
        {
            case -1:
                if (currFlapPos.y > (startFlapPos.y + minRange))
                {
                    flapRB.transform.Translate(-transform.up * Time.deltaTime * bounceSpeed);
                }
                else
                {
                    directionB = 1;
                }
                break;
            case 1:
                if (currFlapPos.y < (startFlapPos.y + maxRange))
                {
                    flapRB.transform.Translate(transform.up * Time.deltaTime * bounceSpeed);
                }
                else
                {
                    directionB = -1;
                }
                break;
        }
    }

    private void Attack()
    {
        canAttack = false;
        aceFlapPos = GameObject.Find("FlapEnemy").GetComponent<Transform>().position;

        while(currFlapPos.y > (aceFlapPos.y + dropRange))
        {
            flapRB.transform.Translate(-transform.up * Time.deltaTime * bounceSpeed);
        }
        for(int i = 0; i < 100; i++)
        {
            Debug.Log("Flap is biting and chilllin'");
        }
        while(currFlapPos.y < aceFlapPos.y)
        {
            flapRB.transform.Translate(transform.up * Time.deltaTime * bounceSpeed);
        }
        flapRB.transform.position = aceFlapPos;
        canAttack = true;
    }

    public void SetOverPlayer(bool value)
    {
        overPlayer = value;
    }

    public bool GetCanAttack()
    {
        return canAttack;
    }

    public void ProjectileMovement()
    {
       if (turretProjectileRB.velocity.x == 0f && turretProjectileRB.velocity.y == 0f)
        {
            target = (playerRB.position - turretProjectileRB.position);
            target = target.normalized;
            turretProjectileRB.AddForce(target * projectileSpeed, ForceMode2D.Impulse);
        }
    }
}
