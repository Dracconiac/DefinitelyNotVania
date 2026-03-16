using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpener : MonoBehaviour
{
    Animator animator;
    bool opened;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Open()
    {
        if (!opened)
        {
            animator.Play("Open", 0, 0);
            opened = true;
        }

    }

}
