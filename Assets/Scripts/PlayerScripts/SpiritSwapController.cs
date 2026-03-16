using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritSwapController : MonoBehaviour
{
    [SerializeField] Rigidbody2D spiritSwap;
    CharacterController characterController;
    Rigidbody2D spirit;
    Vector3 originalPosition;
    Vector3 originalScale;
    bool spiritIsPlaced;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void OnSpiritSwap()
    {
        if (!spiritIsPlaced)
        {
            spirit = Instantiate(spiritSwap, transform.position, transform.rotation);
            spirit.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            originalPosition = transform.position;
            originalScale = transform.localScale;
            spiritIsPlaced = true;
        }
        else
        {
            spirit.GetComponent<SpiritSwap>().Destroy();
            transform.position = originalPosition;
            transform.localScale = originalScale;
            characterController.rb.velocity = Vector3.zero; // prevents player character from continual acceleration when teleported back in midair while falling
            spiritIsPlaced = false;
        }
    }

    void OnSpiritSwapCancel()
    {
        if (spirit != null)
        {
            spirit.GetComponent<SpiritSwap>().Destroy();
            spiritIsPlaced = false;
        }
    }
}
