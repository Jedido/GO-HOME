using System.Collections;
using UnityEngine;

// Red Slime AI
// Movement: chooses a cardinal direction towards the player and moves there for a set amount of time, 
// then chooses a new direction. If it hits a wall, chooses a new direction immediately.
// Attack: 4 cardinal directions
public class BattleRedSlime : BattleCPU {
    private static readonly float speed = 2f;
    private static readonly float walkCooldown = 0.3f;
    private static readonly float shotCooldown = 0.8f;
    private static readonly float shotSpeed = 4f;
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
            EnemyProjectile up = Spawn(SProj).GetComponent<EnemyProjectile>();
            EnemyProjectile down = Spawn(SProj).GetComponent<EnemyProjectile>();
            EnemyProjectile left = Spawn(SProj).GetComponent<EnemyProjectile>();
            EnemyProjectile right = Spawn(SProj).GetComponent<EnemyProjectile>();
            up.InitialVelocity = new Vector2(0, 1f) * shotSpeed;
            down.InitialVelocity = new Vector2(0, -1f) * shotSpeed;
            left.InitialVelocity = new Vector2(-1f, 0) * shotSpeed;
            right.InitialVelocity = new Vector2(1f, 0) * shotSpeed;
        }

        // Movement
        Velocity = direction;
        if (walkTimer < Time.time)
        {
            walkTimer = Time.time + walkCooldown;
            Vector2 dir = PlayerManager.player.battleAlien.transform.localPosition - transform.localPosition;
            float x = Mathf.Abs(dir.x);
            Vector2 dirX = new Vector2(dir.x, 0);
            float y = Mathf.Abs(dir.y);
            Vector2 dirY = new Vector2(0, dir.y);
            if (x > y)
            {
                SetDirection(CanMove(dirX) ? dirX : CanMove(dirY) ? dirY : CanMove(-dirY) ? -dirY : -dirX);
            } else
            {
                SetDirection(CanMove(dirY) ? dirY : CanMove(dirX) ? dirX : CanMove(-dirX) ? -dirX : -dirY);
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
