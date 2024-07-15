using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Move info")]
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float jumpForce = 1;


    [SerializeField] private float xInput;

    [SerializeField] private float dashDuration = 1;
    [SerializeField] private float dashTime = 1;
    [SerializeField] private float dashSpeed = 1;
    [SerializeField] private float dashCooldown = 1;
    private float dashCooldownTimer = 1;

    private bool isAttacking;
    private int comboCounter;
    [SerializeField] private float comboTimeWindow;
    [SerializeField] private float comboTime = .3f;

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        comboTimeWindow -= Time.deltaTime;

       
        Movement();
        CheckInput();
        TurnController();
        AnimatorControllers();
    }



    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttackCombo();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashAbility();
        }
    }

    private void StartAttackCombo()
    {
        if (!isGrounded)
            return;

        if (comboTimeWindow < 0)
        {
            comboCounter = 0;
        }

        isAttacking = true;
        comboTimeWindow = comboTime;
    }

    private void DashAbility()
    {

        if(dashCooldownTimer < 0 && !isAttacking)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }

    private void Movement()
    {
        if (isAttacking) {
            rb.velocity = new Vector2(0, 0);
        }
        else if (dashTime > 0) { 
            // rb.velocity = new Vector2(xInput * dashSpeed, rb.velocity.y); this one still makes you fall down while dashing
            rb.velocity = new Vector2(facinDir * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
        
    }

    private void Jump()
    {
        if (isGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        
    }

    private void AnimatorControllers() {

        bool isMovin = rb.velocity.x != 0;


        anim.SetFloat("yVelocity", rb.velocity.y);

        anim.SetBool("isMoving", isMovin);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);
    }



    private void TurnController() {
        if (rb.velocity.x > 0 && !facingRight) {
            TurnCharacter();
        }
        else if (rb.velocity.x < 0 && facingRight)
        {
            TurnCharacter();
        }
    }



    public void AttackingOver()
    {
        isAttacking = false;
        comboCounter++;
        if (comboCounter > 2)
        {
            comboCounter = 0;
        }
    }
}
