﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// In charge of movement and attacks during battle phase
public abstract class BattleCPU : MonoBehaviour {
    private int health;
    private Rigidbody2D rb2d;
    private Enemy e;
    private bool invulnerable;
    protected GameObject SProj, SNRProj, SPProj, SPNRProj, WProj;
    protected SpriteRenderer sprite;
    protected Sprite Sprite
    {
        set { sprite.sprite = value; }
    }

    protected bool Invincible
    {
        get { return invulnerable; }
        set {
            invulnerable = value;
            if (invulnerable)
            {
                sprite.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
            } else
            {
                sprite.color = new Color(1, 1, 1, 1f);
            }
        }
    }
    public int HP
    {
        get { return health; }
    }

    // Death animation
    private bool dying;
    private float fade;

    protected Vector2 Velocity
    {
        get { return rb2d.velocity; }
        set { rb2d.velocity = value; }
    }

    protected void Start()
    {
        health = GetHealth();
        rb2d = GetComponent<Rigidbody2D>();
        SProj = SpriteLibrary.library.SProjectile;
        SPProj = SpriteLibrary.library.SPProjectile;
        SNRProj = SpriteLibrary.library.SNRProjectile;
        SPNRProj = SpriteLibrary.library.SPNRProjectile;
        WProj = SpriteLibrary.library.WProjectile;
        sprite = GetComponentInChildren<SpriteRenderer>();
        invulnerable = false;
    }

    protected void FixedUpdate()
    {
        if (PlayerManager.player.battle.GetComponent<BattleController>().Active)
        {
            if (dying)
            {
                fade -= 0.05f;
                sprite.color = new Color(1, 1, 1, fade);
                if (fade <= 0)
                {
                    e.Die();
                }
            }
            else
            {
                UpdateCPU();
            }
        }
    }

    protected abstract int GetHealth();
    protected abstract void UpdateCPU();

    // 0 is right, 1 is up, 2 is left, else is down
    protected bool CanMove(int direction)
    {
        Vector2 dir;
        switch (direction)
        {
            case 0: dir = new Vector2(1, 0); break;
            case 1: dir = new Vector2(0, 1); break;
            case 2: dir = new Vector2(-1, 0); break;
            default: dir = new Vector2(0, -1); break;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 0.7f);
        return hit.collider == null || !hit.collider.tag.Equals("Wall");
    }

    protected bool CanMove(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 0.3f);
        return hit.collider == null || !hit.collider.tag.Equals("Wall");
    }

    protected GameObject Spawn(GameObject obj)
    {
        return Instantiate(obj, transform.position, Quaternion.identity, transform.parent);
    }

    public void SetEnemy(Enemy enemy)
    {
        e = enemy;
    }
	
    public virtual void Hit(int damage)
    {
        if (!invulnerable && !dying)
        {
            health -= damage;
            if (health <= 0)
            {
                dying = true;
                fade = 1;
                rb2d.velocity = new Vector2();
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            // Enable when iframes are added
            PlayerManager.player.battleAlien.GetComponent<PlayerBattleController>().Hit(1);
        }
    }
}
