using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    ScorpionController scorpionController;
    EnemyCombat enemyCombat;
    public bool isDamaged;
    public bool isKnockedback;

    void Start()
    {
        GetComponents();
    }


    void Update()
    {

    }

    public void TakeDamage()
    {
        if (isDamaged)
        {
            Die();
        }
        else
        {
            isDamaged = true;

            scorpionController.animator.Play(scorpionController.HitStateHash, 0, 0);

            if (!scorpionController.isIdling && !enemyCombat.isAttacking)
            {
                StartCoroutine(scorpionController.Idle(2f));
            }

        }
    }

    void Die()
    {
        scorpionController.animator.Play(scorpionController.DeathStateHash, 0, 0);
        scorpionController.animator.SetBool("isPatroling", false);
        scorpionController.movementSpeed = 0f;
        StopAllCoroutines();
        Physics2D.IgnoreLayerCollision((int)Enums.CollisionLayers.Player, (int)Enums.CollisionLayers.Enemy);
    }

    void GetComponents()
    {
        scorpionController = this.gameObject.GetComponent<ScorpionController>();
        enemyCombat = this.gameObject.GetComponent<EnemyCombat>();
    }
}
