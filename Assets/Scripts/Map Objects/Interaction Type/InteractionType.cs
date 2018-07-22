using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Method of interaction with the interactable
// Includes pressure plate, switch, and more
public abstract class InteractionType : MonoBehaviour {
    private Interactable interactable;
    private bool active;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        active = true;
    }

    public void Activate()
    {
        if (active)
        {
            interactable.Interact();
        }
    }

    public void Disable()
    {
        interactable.Reset();
        active = false;
    }

    public void Reset()
    {
        if (active)
        {
            interactable.Reset();
        }
    }
}
