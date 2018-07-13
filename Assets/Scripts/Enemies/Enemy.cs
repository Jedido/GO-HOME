using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    private int damage, health;
    private float movespeed;
    private bool active;
    public int ATK
    {
        get { return damage; }
    }
    public int HP
    {
        get { return health; }
    }
    public float MS
    {
        get { return movespeed; }
    }
    public bool Active
    {
        get { return active; }
    }

    private float detectionRadius;  // when enemy is alerted of player presence
    private Vector3 direction;
    private Rigidbody2D rb2d;

    protected abstract int GetATK();
    protected abstract int GetHP();
    protected abstract float GetMS();
    protected abstract float GetRadius();
    protected abstract void Die();

    // Since Enemy is never created as a gameobject, this is used instead of Unity's Start()
    protected void Init () {
        damage = GetATK();
        health = GetHP();
        movespeed = GetMS();
        active = false;
        detectionRadius = GetRadius();
        direction = new Vector3();
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	protected void Update () {
        float dist = new Vector3(PlayerManager.player.Position.x - transform.position.x,
            PlayerManager.player.Position.y - transform.position.y).magnitude;
        if (dist < detectionRadius)
        {
            active = true;
        }

        rb2d.velocity = direction;
    }

    protected void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    public float GetSpeed()
    {
        return rb2d.velocity.magnitude;
    }

    public void Hit(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            // TODO: drop loot
            Destroy(gameObject);
        }
    }
}
