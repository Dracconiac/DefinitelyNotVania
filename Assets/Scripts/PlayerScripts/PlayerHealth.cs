using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    CharacterController characterController;
    LevelLoader levelLoader;
    public int HP;
    const int maxHP = 3;

    void Awake()
    {
        characterController = this.gameObject.GetComponent<CharacterController>();
        levelLoader = this.gameObject.GetComponent<LevelLoader>();
        HP = maxHP;
        FindObjectOfType<PlayerHealthBar>().UpdateHealthBar(HP);
    }

    IEnumerator Die(float restartDelay)
    {
        //Physics2D.IgnoreLayerCollision((int)Enums.CollisionLayers.Player, (int)Enums.CollisionLayers.Enemy);
        characterController.animator.Play(characterController.DeathStateHash, 0, 0);
        characterController.input.DeactivateInput();
        FindObjectOfType<GameManager>().ProcessPlayerDeath();
        yield return new WaitForSecondsRealtime(restartDelay);
        levelLoader.RestartLevel();
    }

    public void TakeDamage(int damage = 1)
    {
        if (characterController.playerBodyCollider.IsTouchingLayers(characterController.LMEnemy))
        {
            HP--;
            FindObjectOfType<PlayerHealthBar>().UpdateHealthBar(HP);
            characterController.animator.Play(characterController.HurtStateHash, 0, 0);
        }
        
        if (HP == 0 || characterController.playerBodyCollider.IsTouchingLayers(characterController.LMWater))
        {
            StartCoroutine(Die(1f));
        }
    }

}
