using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    ScorpionController scorpionController;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    CircleCollider2D stingerCollider;
    [SerializeField] Transform attackPoint;

    int playerHP = 1;

    public float attackRange;
    public bool isAttacking;

    void Start()
    {
        GetComponents();
    }

    void Update()
    {
        
        if (stingerCollider.IsTouchingLayers(scorpionController.LMPlayer) && !isAttacking && !scorpionController.isIdling)
        {
            if (playerHP != 0)
            {
                StartCoroutine(Attack());
            }
        }
    }

    void DealDamage()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, scorpionController.LMPlayer);

        if (hitPlayer != null)
        {
            playerHealth = hitPlayer.GetComponent<PlayerHealth>();
            hitPlayer.GetComponent<PlayerHealth>().TakeDamage();
            playerHP = playerHealth.HP;
        }
    }
    public void Knockback(float forceX, float forceY)
    {
        enemyHealth.isKnockedback = true;
        if (!enemyHealth.isDamaged)
        {
            scorpionController.rb.velocity = new Vector2(forceX, forceY);
        }
    }

    void GetComponents()
    {
        scorpionController = this.gameObject.GetComponent<ScorpionController>();
        stingerCollider = GetComponent<CircleCollider2D>();
        enemyHealth = this.gameObject.GetComponent<EnemyHealth>();
    }


    //Coroutines
        IEnumerator Attack()
    {
        isAttacking = true;

        float originalSpeed = scorpionController.movementSpeed;
        scorpionController.movementSpeed = 0f;
        scorpionController.animator.SetBool("isPatroling", false);
        scorpionController.animator.Play(scorpionController.AttackStateHash, 0, 0);
        yield return new WaitForSeconds(scorpionController.attackDelay);
        scorpionController.animator.SetBool("isPatroling", true);
        scorpionController.movementSpeed = originalSpeed;


        isAttacking = false;
    }
}
