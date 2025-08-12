using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3.5f;
    [SerializeField] float climbSpeed = 2f;
    [SerializeField] float jumpForce = 5f;
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    Collider2D playerCollider;
    LayerMask layerMaskGround;
    LayerMask layerMaskLadder;
    bool isRunning = false;
    bool isClimbing = false;
    int JumpStateHash;
    int ClimbStateHash;
    int jumpCount = 0;

    float initGravity = 1f;
    float ladderGravity = 0f;
    float stopAnimation = 0f;
    float restartAnimation = 1f;


    void Start()
    {
        layerMaskGround = LayerMask.GetMask("Ground");
        layerMaskLadder = LayerMask.GetMask("Ladders");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        SetAnimationHash();
    }

    void Update()
    {
        isRunning = Mathf.Abs(moveInput.x) > Mathf.Epsilon;
        isClimbing = Math.Abs(moveInput.y) > Mathf.Epsilon;
        if (playerCollider.IsTouchingLayers(layerMaskGround))
        {
            jumpCount = 0;
        }
        StartRunning();
        FlipPlayer();
        ClimbLadder();
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
        if (playerCollider.IsTouchingLayers(layerMaskGround))
        {
            animator.SetBool("isRunning", isRunning);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void FlipPlayer()
    {
        if (isRunning)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (playerCollider.IsTouchingLayers(layerMaskLadder))
        {
            rb.gravityScale = ladderGravity;
            Vector2 climbVelocity = new Vector2(rb.velocity.x, moveInput.y * climbSpeed);
            rb.velocity = climbVelocity;
            animator.SetBool("isClimbing", true);
            if (!isClimbing)
            {
                animator.speed = stopAnimation;
            }
            else
            {
                animator.speed = restartAnimation;
            }
        }
        else
        {
            rb.gravityScale = initGravity;
            animator.SetBool("isClimbing", false);
            animator.speed = restartAnimation;
            return;
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

    void SetAnimationHash()
    {
        JumpStateHash = Animator.StringToHash("Base Layer.jump");
        ClimbStateHash = Animator.StringToHash("Base Layer.Ladder");
    }
}
