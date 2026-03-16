using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{

    Animator animator;
    public bool isSwitchedOn;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void switchOn()
    {
        animator.SetTrigger("switchedOn");
        isSwitchedOn = true;
    }

    public void switchOff()
    {
        animator.SetTrigger("switchedOff");
        isSwitchedOn = false;
    }
}
