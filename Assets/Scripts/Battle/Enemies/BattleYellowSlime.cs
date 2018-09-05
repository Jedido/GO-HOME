using System.Collections;
using UnityEngine;

// Yellow Slime AI
// Movement: moves towards the player
// Attack: lays down mines
public class BattleYellowSlime : BattleCPU {
    private static readonly float speed = 3.5f;
    private static readonly float walkCooldown = 0.3f;
    private static readonly float shotCooldown = 0.8f;
    // private static readonly float shotSpeed = 0f;
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
            Spawn(SNRProj);
        }

        // Movement
        Velocity = direction;
        if (walkTimer < Time.time)
        {
            walkTimer = Time.time + walkCooldown;
            Vector2 dir = PlayerManager.player.battleAlien.transform.localPosition - transform.localPosition;
            SetDirection(dir);
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
