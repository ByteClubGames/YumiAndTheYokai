using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YumiMovement : MonoBehaviour {
    /// <summary>
    /// Includes all declarations for animator, sprite renderer, and integers for storing the
    /// animator state name hashes.
    /// </summary>
    #region Animation State Declarations
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private int deathHash;
    private int fallHash;
    private int idleHash;
    private int jumpHash;
    private int landingHash;    
    private int runHash;
    private int takeDamageHash;
    private int turnHash;
    #endregion





    // Use this for initialization
    void Start () {
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        calculateAnimationStateHash();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        
    }

    private void calculateAnimationStateHash()
    {
        deathHash = Animator.StringToHash("yumiDeath");
        fallHash = Animator.StringToHash("yumiFall");
        idleHash = Animator.StringToHash("yumiIdle");
        jumpHash = Animator.StringToHash("yumiJump");
        landingHash = Animator.StringToHash("yumiLanding");
        runHash = Animator.StringToHash("yumiRun");
        takeDamageHash = Animator.StringToHash("yumiTakeDamage");
        turnHash = Animator.StringToHash("yumiTurn");
    }
}
