using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SpiritAttack : MonoBehaviour
{
    [SerializeField] Transform attackArea;
    [SerializeField] float KBPower;
    public float attackRange;
    Rigidbody2D rb;

    LayerMask LMEnemy;

    float spawnPosition;
    float travelDistance = 2f;
    float deltaMovement;

    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.Play(Animator.StringToHash("Base Layer.Dash NoDust"), 0, 0);
        spawnPosition = rb.position.x;
        LMEnemy = LayerMask.GetMask("Enemy");
        Destroy(gameObject, 2f);
    }
    void Update()
    {
        deltaMovement = Mathf.Abs(rb.position.x - spawnPosition);
        if (deltaMovement >= travelDistance)
        {
            rb.velocity = new Vector2(0f, 0f);
        }
    }

    void DealDamage()
    {
        Collider2D hitEnemy = Physics2D.OverlapCircle(attackArea.position, attackRange, LMEnemy);

        if (hitEnemy != null)
        {

            hitEnemy.GetComponent<EnemyCombat>().Knockback(Mathf.Sign(transform.localScale.x) * KBPower, KBPower);
            hitEnemy.GetComponent<EnemyHealth>().TakeDamage();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                rb.velocity = new Vector2(0f, 0f);
                break;
            case "Enemy":
                DealDamage();
                break;
        }
    }
}
