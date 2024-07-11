using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float jumpForce = 1;
    [SerializeField] private float xInput;

    private int facinDir = 1;
    private bool facingRight = true;
    
    private bool isGrounded;
    [SerializeField] private float groundCheckDistance = 1;
    [SerializeField] private LayerMask whatIsGround = 1;

    [SerializeField] private float dashDuration = 1;
    [SerializeField] private float dashTime = 1;
    [SerializeField] private float dashSpeed = 1;
    [SerializeField] private float dashCooldown = 1;
    private float dashCooldownTimer = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        CollisionChecks();
        Movement();
        CheckInput();
        TurnController();
        AnimatorControllers();
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashAbility();
        }
    }

    private void DashAbility()
    {
        if(dashCooldownTimer < 0)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }

    private void Movement()
    {
        if (dashTime > 0) { 
            // rb.velocity = new Vector2(xInput * dashSpeed, rb.velocity.y); this one still makes you fall down while dashing
            rb.velocity = new Vector2(xInput * dashSpeed, 0);
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

        anim.SetBool("isMoving", isMovin);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void TurnCharacter() {
        facinDir = facinDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, 
            new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }

}
