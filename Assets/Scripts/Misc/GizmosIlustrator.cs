using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosIlustrator : MonoBehaviour
{
    [SerializeField] Transform attackArea;
    [SerializeField] PlayerCombat playerCombat;
    [SerializeField] EnemyCombat enemyCombat;
    void Reset()
    {
        playerCombat = this.gameObject.GetComponent<PlayerCombat>();
        enemyCombat = this.gameObject.GetComponent<EnemyCombat>();
    }

    public void OnDrawGizmos()
    {
        // Attack range circle
        if (playerCombat != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(attackArea.position, playerCombat.attackRange);          
        }

        if (enemyCombat != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(attackArea.position, enemyCombat.attackRange);
        }

    }
}