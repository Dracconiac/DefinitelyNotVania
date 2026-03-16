using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{

    Slider healthBar;

    void Awake()
    {
        healthBar = GetComponent<Slider>();
    }

    public void UpdateHealthBar(int playerHP)
    {
        healthBar.value = playerHP;
    }

}
