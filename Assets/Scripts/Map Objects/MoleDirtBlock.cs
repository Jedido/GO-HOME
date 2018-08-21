using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleDirtBlock : MonoBehaviour {
    public Sprite[] sprites;
    private SpriteRenderer sprite;
    private static readonly float spikeCooldown = 1.4f;
    private float spikeTimer;
    private Collider2D col;
    private bool spike;
    public bool Spike
    {
        set
        {
            if (!wall)
            {
                spike = value;
                if (spike)
                {
                    sprite.sprite = sprites[2];
                    spikeTimer = Time.time + spikeCooldown;
                }
                else
                {
                    sprite.sprite = sprites[1];
                }
            }
        }
    }
    private bool wall;
    public bool Block
    {
        get
        {
            return wall;
        }
        set
        {
            wall = value;
            if (wall)
            {
                spike = false;
                sprite.sprite = sprites[0];
                col.isTrigger = false;
            }
            else
            {
                sprite.sprite = sprites[1];
                col.isTrigger = true;
            }
        }
    }

    // Use this for initialization
    void Start () {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = sprites[1];
        col = GetComponent<Collider2D>();
	}

    private void Update()
    {
        if (spike && spikeTimer < Time.time)
        {
            Spike = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (spike && collision.tag.Equals("Player"))
        {
            collision.GetComponent<PlayerController>().Hit(1, true);
        }
    }
}
