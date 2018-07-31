using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigSpot : MonoBehaviour, Interactable {
    public GameObject hide;

    private void Start()
    {
        hide.SetActive(false);
    }

    public void Interact()
    {
        if (PlayerManager.player.HasKeyItem((int)PlayerManager.KeyItems.Shovel))
        {
            hide.SetActive(true);
            Destroy(gameObject);
        } else
        {
            // TODO: display "I cannot do anything with this yet." or "?"
        }
    }

    public void Reset()
    {
        // Nothing happens
    }
}
