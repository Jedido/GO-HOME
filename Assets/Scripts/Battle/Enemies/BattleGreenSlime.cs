using System.Collections;
using UnityEngine;

// Green Slime AI
// Movement: Nothing
// Attack: Phases through walls. Also hides inside blocks
public class BattleGreenSlime : BattleCPU
{
    private static readonly float speed = 0f;
    private static readonly float shotCooldown = 1f;
    private static readonly float shotSpeed = 3f;
    private float walkTimer, shotTimer;
    private Vector2 direction;

    protected override int GetHealth()
    {
        return 1;
    }

    // Update is called once per frame
    protected override void UpdateCPU()
    {
        // Attack
        if (shotTimer < Time.time)
        {
            Vector2 dir = PlayerManager.player.battleAlien.transform.localPosition - transform.localPosition;
            dir = dir.normalized;
            shotTimer = Time.time + shotCooldown;
            EnemyProjectile shot = Instantiate(smallProj, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
            shot.InitialVelocity = dir * shotSpeed;
            shot.Phase = true;
        }

        // Movement
        // None
    }
}
