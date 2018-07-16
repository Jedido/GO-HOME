using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    public int ATK
    {
        get { return GetATK(); }
    }
    public int HP
    {
        get { return GetHP(); }
    }
    public float MS
    {
        get { return GetMS(); }
    }
    public float Weight
    {
        get { return GetKnockback(); }
    }
    private bool active;
    public bool Active
    {
        get { return active; }
    }

    private float detectionRadius;  // when enemy is alerted of player presence
    private Vector3 direction;
    private Vector3 knockbackDir;
    private Rigidbody2D rb2d;

    public abstract int GetID();
    public enum EnemyID { Slime, };
    protected abstract int GetATK();
    protected abstract int GetHP();
    protected abstract float GetMS();
    protected abstract float GetKnockback();
    protected abstract float GetRadius();
    protected abstract void Die();

    // Since Enemy is never created as a gameobject, this is used instead of Unity's Start()
    protected void Start () {
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

        knockbackDir *= 0.9f;
        rb2d.velocity = direction + knockbackDir;
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
        // health -= damage;
        if (HP < 0)
        {
            // TODO: drop loot
            Destroy(gameObject);
        } else
        {
            // Get knocked back
            knockbackDir += new Vector3(PlayerManager.player.Position.x - transform.position.x,
                PlayerManager.player.Position.y - transform.position.y).normalized * -Weight;
        }
    }
}
