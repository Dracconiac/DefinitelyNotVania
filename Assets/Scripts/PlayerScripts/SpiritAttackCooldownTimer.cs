using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SpiritAttackCooldownTimer : MonoBehaviour
{
    [SerializeField] Image cooldownTimerImage;
    [SerializeField] TMP_Text cooldownText;
    public float remainingCoolDown;
    public bool isOnCooldown;
    public float fillFraction;
    float cooldown = 3f;

    void Start()
    {
        cooldownTimerImage.fillAmount = 0.0f;
    }

    void Update()
    {
        if (isOnCooldown)
        {
            UpdateTimer();
            if (remainingCoolDown > 0)
            {
                cooldownText.text = Mathf.RoundToInt(remainingCoolDown).ToString();
            }
            else
            {
                cooldownText.text = null;
            }
            cooldownTimerImage.fillAmount = fillFraction;
        }
    }

    void UpdateTimer()
    {
        remainingCoolDown -= Time.deltaTime;

        if (remainingCoolDown > 0)
        {
            fillFraction = remainingCoolDown / cooldown;
        }
        else
        {
            fillFraction = 0f;
        }

    }
}
