using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Takes another interactable and uses it
// Looks like a switch
public class Remote : MonoBehaviour, Interactable {
    public GameObject remoteObject;
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private bool active;

    private void Start()
    {
        active = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    public void SetRemoteObject(GameObject remote)
    {
        remoteObject = remote;
    }

    public void Interact()
    {
        if (!active)
        {
            active = true;
            spriteRenderer.sprite = sprites[1];
            remoteObject.GetComponent<Interactable>().Interact();
        } else
        {
            Reset();
        }
    }

    public void Reset()
    {
        active = false;
        spriteRenderer.sprite = sprites[0];
        remoteObject.GetComponent<Interactable>().Reset();
    }
}
