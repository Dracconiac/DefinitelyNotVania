using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(playerHealth.HP); // instant death regardless of remaining HP
                break;
            case "Enemy":
                collision.GetComponent<EnemyHealth>().TakeDamage();
                break;
        }
    }
}
