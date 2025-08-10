using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3.5f;
    [SerializeField] float jumpForce = 5f;
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    Collider2D playerCollider;
    LayerMask layerMaskGround;
    bool isRunning = false;

    int JumpStateHash;
    int jumpCount = 0;


    void Start()
    {
        layerMaskGround = LayerMask.GetMask("Ground");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        JumpStateHash = Animator.StringToHash("Base Layer.jump");
    }

    void Update()
    {
        isRunning = Mathf.Abs(moveInput.x) > Mathf.Epsilon;
        if (playerCollider.IsTouchingLayers(layerMaskGround))
        {
            jumpCount = 0;
        }
        StartRunning();
        FlipPlayer();
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && jumpCount < 1) //double jump
        {
            jumpCount++;
            animator.SetBool("isMidair", true);
            rb.velocity = new Vector2(0f, jumpForce);
            animator.Play(JumpStateHash, 0, 0);
        }
    }


    void StartRunning()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;
        animator.SetBool("isRunning", isRunning);
    }

    void FlipPlayer()
    {
        if (isRunning)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            animator.SetBool("isMidair", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            animator.SetBool("isMidair", true);
        }
    }
}
