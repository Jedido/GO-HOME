using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, Interactable {
    public Sprite[] sprites;
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = sprites[0];
    }

    public void Interact()
    {
        Reward script = GetComponent<Reward>();
        if (script != null)
        {
            sprite.sprite = sprites[1];
            script.GrantReward();
            Destroy(script);
        }
    }

    public void Reset()
    {
        // nothing happens
    }
}
