using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBlueSlime : BattleCPU {
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
            EnemyProjectile shot = Instantiate(smallProj, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
            shot.InitialVelocity = dir * shotSpeed;
        }

        // Movement
        Velocity = dir * speed;
    }
}
