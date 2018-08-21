using UnityEngine;

public abstract class BattleMoleKing : BattleCPU {
    private int phase;
    private static readonly float projCooldown = 1f;
    private float moveTimer, projTimer;
    private int corner;

    private readonly Vector2[] corners = { new Vector2(5f, -3.5f), new Vector2(5f, 3.5f), new Vector2(-5f, 3.5f), new Vector2(-5f, -3.5f) };

    protected abstract GameObject GetProjectile();
    protected abstract int GetPhase();

    new protected void Start()
    {
        base.Start();
        corner = 0;
        phase = GetPhase() + 1;
    }

    protected override void UpdateCPU()
    {
        if (!Invincible)
        {
            if (projTimer < Time.time)
            {
                projTimer = Time.time + projCooldown / phase;
                GameObject proj = Spawn(GetProjectile());
                Vector3 dir = PlayerManager.player.battleAlien.transform.position - transform.position;
                proj.GetComponent<EnemyProjectile>().InitialVelocity = dir.normalized * 4f;
            }

            if (moveTimer < Time.time)
            {
                if (Velocity.magnitude != 0)
                {
                    moveTimer = Time.time + 3f / phase;
                    Velocity = Vector2.zero;
                }
                else
                {
                    corner = (corner + 1) % 4;
                    Vector3 dist = ((Vector3)corners[corner] - transform.localPosition);
                    moveTimer = Time.time + dist.magnitude / phase / 3f;
                    Velocity = dist.normalized * phase * 3;
                }
            }
        }
    }

    protected override int GetHealth()
    {
        return 10;
    }

    public override void Hit(int damage)
    {
        if (damage >= HP)
        {
            // TODO: run away
            Invincible = true;
            Velocity = (transform.localPosition - new Vector3(0, 0)).normalized;
        }
        else
        {
            base.Hit(damage);
        }
    }
}