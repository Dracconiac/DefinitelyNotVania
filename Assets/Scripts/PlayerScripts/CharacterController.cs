using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 3.5f;
    [SerializeField] float dashSpeed = 5.78f;
    [SerializeField] float dashingTime = 0.3f;
    [SerializeField] float dashingCooldown = 1f;
    [SerializeField] float climbSpeed = 2f;
    [SerializeField] float jumpForce = 5f;
    Vector2 moveInput;
    public bool isFacedright = true;
    bool isRunning;
    bool isClimbing;
    public bool isDashing;
    bool canDash = true;
    int jumpCount = 0;
    float initGravity;
    float ladderGravity = 0f;
    float flipOffset = 0.5f; //offsets the sprite on x axis by 0.5f to fix clipping through walls


    [Header("Component references")]
    public PlayerInput input;
    public Rigidbody2D rb;

    [Header("Colliders")]
    public Collider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;


    [Header("LayerMasks")]
    LayerMask LMGround;
    LayerMask LMLadder;
    public LayerMask LMEnemy;
    public LayerMask LMWater;


    [Header("Animations")]
    public Animator animator;
    int JumpStateHash;
    int FallStateHash;
    int DashStateHash;
    public int HurtStateHash;
    public int AttackStateHash;
    int MidairDashStateHash;
    public int DeathStateHash;
    float stopAnimation = 0f;
    float restartAnimation = 1f;
    public bool onLadder;



    void Start()
    {
        GetLayerMasks();
        SetAnimationHash();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        input = GetComponent<PlayerInput>();
        initGravity = rb.gravityScale;
    }

    void FixedUpdate()
    {
        FlipPlayer();
    }

    void Update()
    {
        if (isDashing) // prevents velocity overrides during dash
        {
            return;
        }
        isRunning = Mathf.Abs(moveInput.x) > Mathf.Epsilon;
        isClimbing = Math.Abs(moveInput.y) > Mathf.Epsilon;

        StartRunning();
        ClimbLadder();
    }

    void StartRunning()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;
        if (playerFeetCollider.IsTouchingLayers(LMGround))
        {
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isMidair", false);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isMidair", true);
        }
    }

    void FlipPlayer()
    {
        if (isRunning)
        {
            float originalOriantation = transform.localScale.x;
            bool isFlipped = MathF.Sign(originalOriantation) != Mathf.Sign(rb.velocity.x);

            if ((Mathf.Sign(rb.velocity.x) == 1) && isFlipped)
            {
                transform.position = new Vector3(transform.position.x + flipOffset, transform.position.y, transform.position.z);
                isFacedright = true;
            }
            else if ((MathF.Sign(rb.velocity.x) == -1) && isFlipped)
            {
                transform.position = new Vector3(transform.position.x - flipOffset, transform.position.y, transform.position.z);
                isFacedright = false;
            }

            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (playerBodyCollider.IsTouchingLayers(LMLadder))
        {
            onLadder = true;
            rb.gravityScale = ladderGravity;
            Vector2 climbVelocity = new Vector2(rb.velocity.x, moveInput.y * climbSpeed);
            rb.velocity = climbVelocity;
            animator.SetBool("isClimbing", true);
            animator.SetBool("isMidair", false);
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
            onLadder = false;
            rb.gravityScale = initGravity;
            animator.SetBool("isClimbing", false);
            animator.speed = restartAnimation;
            return;
        }
    }

    //Input System calls handlers
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && jumpCount < 2 && !onLadder) //double jump
        {
            jumpCount++;
            animator.SetBool("isMidair", true);
            rb.velocity = new Vector2(0f, jumpForce);
        
            animator.Play(JumpStateHash, 0, 0);
        }
    }

    void OnDash()
    {
        if (canDash)
        {
            StartCoroutine(Dash());
            if (playerFeetCollider.IsTouchingLayers(LMGround) && !onLadder)
            {
                animator.Play(DashStateHash, 0, 0);
            }
            else if(!onLadder)
            {
                animator.Play(MidairDashStateHash, 0, 0);
            }
        }
 
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                if (playerFeetCollider.IsTouchingLayers(LMGround))
                { 
                    jumpCount = 0;
                }
                break;
        }
    }

    void SetAnimationHash()
    {
        JumpStateHash = Animator.StringToHash("Base Layer.jump");
        FallStateHash = Animator.StringToHash("Base Layer.Fall");
        DashStateHash = Animator.StringToHash("Base Layer.Dash");
        MidairDashStateHash = Animator.StringToHash("Base Layer.Dash NoDust");
        DeathStateHash = Animator.StringToHash("Base Layer.Death");
        HurtStateHash = Animator.StringToHash("Base Layer.Hurt");
        AttackStateHash = Animator.StringToHash("Base Layer.Attack");
    }

    void GetLayerMasks()
    {
        LMGround = LayerMask.GetMask("Ground");
        LMLadder = LayerMask.GetMask("Ladders");
        LMEnemy = LayerMask.GetMask("Enemy");
        LMWater = LayerMask.GetMask("Water");
    }

    //Coroutines
    IEnumerator Dash()
    {
        canDash = false;
        
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);

        canDash = true;
    }
}
