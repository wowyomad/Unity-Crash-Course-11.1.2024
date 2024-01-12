using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float horizontalSpeed;
    [SerializeField]
    private float jumpForce;


    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        Movement();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }


    void Movement()
    {
        if(float.Equals(rb.velocity, 0) { return; }
        float deltaX = Input.GetAxisRaw("Horizontal") * horizontalSpeed;
        rb.velocity = new Vector2(deltaX, rb.velocity.y);
    }

    void Jump()
    {
       rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
    }

    
}
