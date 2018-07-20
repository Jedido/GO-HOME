using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PressurePlate : MonoBehaviour, Interactable {
    public abstract void Interact();
    public abstract void Reset();  // for animation purposes

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            Interact();
        }  
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            Reset();
        }
    }
}
