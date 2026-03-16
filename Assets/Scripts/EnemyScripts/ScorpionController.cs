using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ScorpionController : MonoBehaviour
{
    EnemyHealth enemyHealth;
    [SerializeField] CharacterController characterController;

    public Rigidbody2D rb;
    PolygonCollider2D enemyFrontCollider; //Might need these in the future to know
    CapsuleCollider2D enemyBackCollider; // what side of enemy I am colliding with

    public LayerMask LMPlayer;
    public Animator animator;

    public float movementSpeed = 1f;
    [SerializeField] float movementRange = 5f;
    float startingPosition;
    float endPosition;
    public float attackDelay = 1f;

    const int facingRight = 1;
    const int facingLeft = -1;

    bool enemyFlipped;
    public bool isIdling;

    public int AttackStateHash;
    public int DeathStateHash;
    public int HitStateHash;

    void Start()
    {
        GetLayerMasks();
        SetAnimationHash();
        GetComponents();
        startingPosition = rb.position.x;
        endPosition = startingPosition - movementRange;
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (!enemyHealth.isKnockedback)
        {
            float deltaMovement = 0f;

            if (enemyFlipped)
            {
                deltaMovement = Mathf.Abs(rb.position.x - endPosition);
                Vector2 enemyVelocity = new Vector2(movementSpeed * facingRight, rb.velocity.y);
                rb.velocity = enemyVelocity;
            }

            if (!enemyFlipped)
            {
                deltaMovement = Mathf.Abs(rb.position.x - startingPosition);
                Vector2 enemyVelocity = new Vector2(movementSpeed * facingLeft, rb.velocity.y);
                rb.velocity = enemyVelocity;
            }

            if (deltaMovement >= movementRange)
            {
                FlipEnemy();
                enemyFlipped = !enemyFlipped;
            }
        }
    }



    void FlipEnemy()
    {
        transform.localScale = new Vector2(rb.transform.localScale.x * -1, 1f);
    }

    void SetAnimationHash()
    {
        AttackStateHash = Animator.StringToHash("Base Layer.ScorpionAttack");
        DeathStateHash = Animator.StringToHash("Base Layer.ScorpionDeath");
        HitStateHash = Animator.StringToHash("Base Layer.ScorpionHit");
    }

    void GetLayerMasks()
    {
        LMPlayer = LayerMask.GetMask("Player");
    }

    void GetComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyFrontCollider = GetComponent<PolygonCollider2D>();
        enemyBackCollider = GetComponent<CapsuleCollider2D>();
        enemyHealth = this.gameObject.GetComponent<EnemyHealth>();

    }



    //Coroutines
    public IEnumerator Idle(float duration)
    {
        isIdling = true;

        float originalSpeed = movementSpeed;
        animator.SetBool("isPatroling", false);
        movementSpeed = 0f;
        yield return new WaitForSeconds(duration);
        movementSpeed = originalSpeed;
        animator.SetBool("isPatroling", true);
        enemyHealth.isKnockedback = false;

        isIdling = false;
    }
}
