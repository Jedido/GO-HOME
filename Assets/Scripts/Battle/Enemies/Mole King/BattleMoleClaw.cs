using UnityEngine;

public class BattleMoleClaw : BattleCPU {
    private Vector3 direction;
    private static readonly float aimCooldown = 0.5f;
    private static readonly float slashCooldown = 3f;
    private float slashTimer, aimTimer;
    private bool aim;

    new protected void Start()
    {
        base.Start();
    }

    protected override int GetHealth()
    {
        return -1;
    }

    public override void Hit(int damage)
    {

    }

    protected override void UpdateCPU()
    {
        Vector3 dir = PlayerManager.player.battleAlien.transform.position - transform.position;
        if (aim)
        {
            float angle = Vector3.Angle(Vector3.down, dir);
            if (dir.x < 0)
            {
                angle = -angle;
            }
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (aim && aimTimer < Time.time)
        {
            aim = false;
            Velocity = dir.normalized * 4f;
            slashTimer = Time.time + slashCooldown;
        }

        if (slashTimer > Time.time)
        {
            dir = Velocity;
            float angle = Vector3.Angle(Vector3.down, dir);
            if (dir.x < 0)
            {
                angle = -angle;
            }
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (!aim && slashTimer < Time.time)
        {
            aim = true;
            aimTimer = Time.time + aimCooldown + Random.value;
        }
    }
}