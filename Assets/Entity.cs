using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;

    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 1;
    [SerializeField] protected LayerMask whatIsGround = 1;
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = 1;


    protected int facinDir = 1;
    protected bool facingRight = true;


    protected bool isGrounded;
    protected bool isWallDetected;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CollisionChecks();
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance * facinDir, whatIsGround);
    }

    protected virtual void TurnCharacter()
    {
        facinDir = facinDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance * facinDir, wallCheck.position.y));
    }
}
