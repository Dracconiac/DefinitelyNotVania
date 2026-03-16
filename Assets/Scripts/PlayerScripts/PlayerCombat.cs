using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack")]
    public float attackRange;
    [SerializeField] SpiritAttackCooldownTimer spiritAttackCooldownTimer;
    [SerializeField] Transform attackArea;
    [SerializeField] float KBPower;
    [SerializeField] Rigidbody2D spiritAttack;
    [SerializeField] float spiritAttackSpeed;
    float spiritAttackCooldown = 3f;
    bool canSpiritAttack = true;

    [SerializeField] CharacterController characterController;

    void Start()
    {
        characterController = this.gameObject.GetComponent<CharacterController>();
    }

    void DealDamage()
    {
        Collider2D hitEnemy = Physics2D.OverlapCircle(attackArea.position, attackRange, characterController.LMEnemy);

        if (hitEnemy != null)
        {
            hitEnemy.GetComponent<EnemyCombat>().Knockback(Mathf.Sign(transform.localScale.x) * KBPower, KBPower);
            hitEnemy.GetComponent<EnemyHealth>().TakeDamage();
        }
    }

    void OnAttack()
    {
        characterController.animator.SetTrigger("isAttacking");
    }

    void OnFire()
    {
        //creates Spirit object
        if (canSpiritAttack)
        {
            StartCoroutine(SpiritAttack());
        }
    }

    //Coroutines

    IEnumerator SpiritAttack()
    {

        canSpiritAttack = false;

        spiritAttackCooldownTimer.remainingCoolDown = spiritAttackCooldown;
        spiritAttackCooldownTimer.isOnCooldown = true;
        Rigidbody2D s = Instantiate(spiritAttack, transform.position, transform.rotation);
        s.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
        s.velocity = new Vector2(transform.localScale.x * spiritAttackSpeed, 0f);
        yield return new WaitForSeconds(spiritAttackCooldown);
        spiritAttackCooldownTimer.isOnCooldown = false;

        canSpiritAttack = true;
    }
}
