using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Internal;

public class Player : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField]
    private float horizontalSpeed;
    [SerializeField]
    private float jumpForce;


    [Header("Collisions")]
    [SerializeField]
    private float groundCheckDistance = 0.2f;
    [SerializeField]
    private float wallCheckDistance = 0.2f;

    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private LayerMask whatIsWall;

    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private bool isFacingWall;


    private float xInput;

    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;


    private int facingDir = 1;
    public bool IsFacingRight => facingDir == 1;
    public bool IsFacingLeft => facingDir == -1;

    private void Awake()
    {
        whatIsGround = LayerMask.GetMask("Ground");
        whatIsWall = LayerMask.GetMask("Wall");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        CollisionChecks();
        InputController();
        Move();
        FlipController();
        AnimationController();
    }

    void InputController()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void AnimationController()
    {
        bool isMoving = xInput != 0;
        anim.SetBool("isMoving", isMoving);
        float yVelocity = rb.velocity.y;
        anim.SetFloat("yVelocity", yVelocity);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void Move()
    {
        if (!isFacingWall)
        {
            float deltaX = xInput * horizontalSpeed;
            rb.velocity = new Vector2(deltaX, rb.velocity.y);

        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void FlipController()
    {
        if (xInput < 0 && IsFacingRight || xInput > 0 && !IsFacingRight)
        {
            Flip();
        }
    }

    private void Jump()
    {
        float deltaY = jumpForce;
        rb.velocity = new Vector2(rb.velocity.x, deltaY);
    }


    private void Flip()
    {
        transform.Rotate(0, 180.0f, 0);
        facingDir *= -1;
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, col.bounds.extents.y + groundCheckDistance, whatIsGround);
        return hit.collider != null;
    }

    private bool IsFacingWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(facingDir, 0), col.bounds.extents.x + wallCheckDistance, whatIsWall);
        return hit.collider != null;
    }

    private void CollisionChecks()
    {
        isGrounded = IsGrounded();
        isFacingWall = IsFacingWall();

        print($"isGrounded: {isGrounded}");
        print($"isFacingWall: {isFacingWall}");
    }
}
