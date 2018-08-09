using System.Collections;
using UnityEngine;

// Red Slime AI
// Movement: chooses a cardinal direction towards the player and moves there for a set amount of time, 
// then chooses a new direction. If it hits a wall, chooses a new direction immediately.
// Attack: 4 cardinal directions
public class BattleRedSlime : BattleCPU
{
    private static readonly float speed = 1.5f;
    private static readonly float shotCooldown = 0.5f;
    private static readonly float shotSpeed = 5f;
    private float shotTimer;

    protected override int GetHealth()
    {
        return 2;
    }

    // Update is called once per frame
    protected override void UpdateCPU()
    {
        Vector2 dir = PlayerManager.player.battleAlien.transform.localPosition - transform.localPosition;
        dir = dir.normalized;
        // Attack
        if (shotTimer < Time.time)
        {
            shotTimer = Time.time + shotCooldown;
            EnemyProjectile shot = Spawn(SProj).GetComponent<EnemyProjectile>();
            shot.InitialVelocity = dir * shotSpeed;
        }

        // Movement
        Velocity = dir * speed;
    }
}
