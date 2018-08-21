using UnityEngine;

public abstract class BattleMoleKing : BattleCPU {
    public Sprite[] phases;
    private int phase;
    protected int Phase
    {
        set {
            phase = value;
            Sprite = phases[phase];
        }
    }
    public GameObject body, left, right;
    protected BattleMoleClaw l, r;

    private static readonly float projCooldown = 1f;
    private float moveTimer, projTimer;
    private int corner;

    private readonly Vector2[] corners = { new Vector2(5f, -3.5f), new Vector2(5f, 3.5f), new Vector2(-5f, 3.5f), new Vector2(-5f, -3.5f) };

    protected abstract GameObject GetProjectile();

    new protected void Start()
    {
        base.Start();
        left.transform.localPosition = new Vector2(-0.5f, 0);
        l = left.GetComponent<BattleMoleClaw>();
        l.Origin = new Vector2(-0.5f, 0);
        right.transform.localPosition = new Vector2(0.5f, 0);
        r = right.GetComponent<BattleMoleClaw>();
        r.Origin = new Vector2(0.5f, 0);
        r.Invert();
        corner = 0;
        transform.localPosition = corners[0];
        transform.rotation = Quaternion.AngleAxis(corner * 90, Vector3.forward);
    }

    protected override void UpdateCPU()
    {
        if (projTimer < Time.time)
        {
            projTimer = Time.time + projCooldown;
            GameObject proj = Spawn(GetProjectile());
            Vector3 dir = PlayerManager.player.battleAlien.transform.position - transform.position;
            proj.GetComponent<EnemyProjectile>().InitialVelocity = dir.normalized * 5f;
        }

        if (moveTimer < Time.time)
        {
            transform.rotation = Quaternion.AngleAxis(corner * 90, Vector3.forward);
            corner = (corner + 1) % 4;
            Vector3 dist = ((Vector3)corners[corner] - transform.localPosition);
            moveTimer = Time.time + dist.magnitude / 3;
            Velocity = dist.normalized * 3;
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
            Velocity = (transform.localPosition - new Vector3(0, 0)).normalized * 3;
        }
        else
        {
            base.Hit(damage);
        }
    }
}