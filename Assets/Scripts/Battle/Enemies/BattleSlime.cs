using System.Collections;
using UnityEngine;

// Generic Slime AI
// Movement: chooses a cardinal direction towards the player and moves there for a set amount of time, 
// then chooses a new direction. If it hits a wall, chooses a new direction immediately.

public class BattleSlime : BattleCPU {
    private static readonly float speed = 2f;
    private static readonly float walkCooldown = 0.3f;
    private static readonly float shotCooldown = 0.8f;
    private float walkTimer, shotTimer;
    private Vector2 direction;

    protected override int GetHealth()
    {
        return 3;
    }
	
	// Update is called once per frame
	protected override void UpdateCPU () {
        // Attack
        if (shotTimer < Time.time)
        {
            shotTimer = Time.time + shotCooldown;
            EnemyProjectile up = Instantiate(smallProj, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
            EnemyProjectile down = Instantiate(smallProj, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
            EnemyProjectile left = Instantiate(smallProj, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
            EnemyProjectile right = Instantiate(smallProj, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
            up.InitialVelocity = new Vector2(0, 5f);
            down.InitialVelocity = new Vector2(0, -5f);
            left.InitialVelocity = new Vector2(-5f, 0);
            right.InitialVelocity = new Vector2(5f, 0);
        }

        // Movement
        Velocity = direction;
        if (walkTimer < Time.time)
        {
            walkTimer = Time.time + walkCooldown;
            Vector2 dir = PlayerManager.player.battleAlien.transform.localPosition - transform.localPosition;
            if (CanMove(dir))
            {
                SetDirection(dir);
            } else
            {
                float x = Mathf.Abs(dir.x);
                Vector2 dirX = new Vector2(dir.x, 0);
                float y = Mathf.Abs(dir.y);
                Vector2 dirY = new Vector2(0, dir.y);
                SetDirection(CanMove(dirX) ? dirX : CanMove(dirY) ? dirY : CanMove(-dirX) ? -dirX : -dirY);
            }
        }
    }

    private void SetDirection(Vector2 dir)
    {
        Velocity = direction = dir.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Wall"))
        {
            walkTimer = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Wall"))
        {
            walkTimer = 0;
        }
    }
}
