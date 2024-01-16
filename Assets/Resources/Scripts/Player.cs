using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Internal;

public class Player : Entity
{

    [Header("Movement")]
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]

    [Header("Dash")]
    private float dashSpeed;
    [SerializeField]
    private float dashDuration;

    [SerializeField]
    private float dashCoolDown;
    private float dashCooldownTimer;
    private float dashTimer = 0.0f;
    private bool IsDashing => dashTimer > 0.0f;
    private bool IsDashOnCooldown => dashCooldownTimer > 0.0f;


    private int comboCount = -1;
    private bool isAttacking = false;

    private float attackDuration;
    [SerializeField]
    private float attackWindow = 1f;



    private float xInput;

    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;


   

    private void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        base.Update();

        dashTimer -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        attackDuration += Time.deltaTime;


        InputController();
        Move();
        FlipController();
        AnimationController();
    }

    void Attack()
    {
        comboCount++;
        if (comboCount > 2)
        {
            comboCount = 0;
        }
        if(attackDuration > attackWindow)
        {
            comboCount = 0;
        }
        attackDuration = 0.0f;
        isAttacking = true;
    }

    public void AttackOver()
    {
        isAttacking = false;
    }

    void InputController()
    {
        if (!IsDashing)
            xInput = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            Jump();
        }


        if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashAbility();
        }


    }

    void DashAbility()
    {
        if (!IsDashOnCooldown)
        {
            dashTimer = dashDuration;
            dashCooldownTimer = dashCoolDown;
        }
    }

    void AnimationController()
    {
        bool isMoving = xInput != 0;
        anim.SetBool("isMoving", isMoving);
        float yVelocity = rb.velocity.y;
        if(!isGrounded)
        {
            anim.SetFloat("yVelocity", yVelocity);

        }
        anim.SetBool("isGrounded", isGrounded);

        anim.SetBool("isDashing", IsDashing);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCount", comboCount);
    }

    private void Move()
    {
        float xSpeed = IsDashing
            ? dashSpeed * facingDir
            : movementSpeed * xInput;
        float ySpeed = IsDashing
            ? 0
            : rb.velocity.y;

        rb.velocity = new Vector2(xSpeed, ySpeed);
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
}
