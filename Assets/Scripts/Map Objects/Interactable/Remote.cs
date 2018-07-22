using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Takes another interactable and uses it
// Looks like a switch
public class Remote : MonoBehaviour, Interactable {
    private Interactable remoteObject;
    private Animator animator;
    private bool active;

    private void Start()
    {
        animator = GetComponent<Animator>();
        active = false;
    }

    public void SetRemoteObject(Interactable remote)
    {
        remoteObject = remote;
    }

    public void Interact()
    {
        if (!active)
        {
            animator.SetTrigger("Switch");
            active = true;
            remoteObject.Interact();
        } else
        {
            Reset();
        }
    }

    public void Reset()
    {
        active = false;
        animator.SetTrigger("Switch");
        remoteObject.Reset();
    }
}
