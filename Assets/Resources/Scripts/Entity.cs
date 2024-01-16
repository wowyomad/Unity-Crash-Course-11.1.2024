using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Collider2D col;
    protected Rigidbody2D rb;
    protected Animator animator;


    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private LayerMask whatIsWall;

    [SerializeField]
    protected bool isGrounded;
    [SerializeField]
    protected bool isFacingWall;

    [Header("Collisions")]
    [SerializeField]
    private float groundCheckDistance = 0.2f;
    [SerializeField]
    private float wallCheckDistance = 0.2f;


    protected int facingDir = 1;
    public bool IsFacingRight => facingDir == 1;
    public bool IsFacingLeft => facingDir == -1;

    protected virtual void Awake()
    {
        whatIsGround = LayerMask.GetMask("Ground");
        whatIsWall = LayerMask.GetMask("Wall");
    }

    protected virtual void Start()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        CollisionChecks();
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, col.bounds.extents.y + groundCheckDistance, whatIsGround);
        return hit.collider != null;
    }

    public bool IsFacingWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(facingDir, 0), col.bounds.extents.x + wallCheckDistance, whatIsWall);
        return hit.collider != null;
    }

    protected void CollisionChecks()
    {
        isGrounded = IsGrounded();
        isFacingWall = IsFacingWall();

        print($"isGrounded: {isGrounded}");
        print($"isFacingWall: {isFacingWall}");
    }

    protected void Flip()
    {
        facingDir *= -1;
        transform.localScale = new Vector2(facingDir, 1);

    }
}
