using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// In charge of movement and attacks during battle phase
public abstract class BattleAI : MonoBehaviour {
    private int health;
    private Rigidbody2D rb2d;
    private Enemy e;
    protected GameObject smallProj, proj, bigProj;
    protected Vector2 Velocity
    {
        set { rb2d.velocity = value; }
    }

    private void Start()
    {
        health = GetHealth();
        rb2d = GetComponent<Rigidbody2D>();
        smallProj = SpriteLibrary.library.SmallProjectile;
    }

    protected abstract int GetHealth();

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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 0.5f);
        return hit.collider == null || !hit.collider.tag.Equals("Wall");
    }

    protected bool CanMove(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 0.5f);
        return hit.collider == null || !hit.collider.tag.Equals("Wall");
    }

    public void SetEnemy(Enemy enemy)
    {
        e = enemy;
    }
	
    public void Hit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            e.Die();
        }
    }
}
