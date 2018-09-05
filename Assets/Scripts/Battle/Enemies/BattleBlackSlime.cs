using UnityEngine;

// Black Slime AI
// Phase 1
// Movement: Stay at right
// Attack: Walls and a line of SNR bullets at the back
// Phase 2 
// Movement: Stay at right
// Attack: Bigger walls and shoots P bullets
// Phase 3 (berserk)
// Movement: Stay at right
// Attack: Faster, smaller walls and fast SNR bullets
public class BattleBlackSlime : BattleCPU
{
    public Sprite angry;

    private int phase;
    private GameObject[] wall;
    private readonly float moveCooldown1 = 2f;
    private readonly float shotCooldown1 = 0.35f;
    private readonly float shotCooldown2 = 0.8f;
    private readonly float phaseCooldown = 1f;
    private float moveTimer, shotTimer1, shotTimer2, phaseTimer;
    private readonly Vector3 initPos = new Vector2(4, 0);
    private static readonly Vector3[] positions = { new Vector2(4, 3), new Vector2(4, -3) };
    private int pos;

    new protected void Start()
    {
        base.Start();
        phase = 0;
        Invincible = true;
        wall = new GameObject[20];
        ProjWall();
    }

    protected override int GetHealth()
    {
        return 12;
    }

    // Update is called once per frame
    protected override void UpdateCPU()
    {
        if (shotTimer1 < Time.time)
        {
            ProjWall();
        }
        if (phase != (12 - HP - 1) / 4)
        {
            phase = (12 - HP - 1) / 4;
            Invincible = true;
            phaseTimer = Time.time + phaseCooldown;
            switch (phase)
            {
                case 0: break;
                case 1:
                    {
                        moveTimer = Time.time + moveCooldown1;
                        Velocity = (initPos - transform.localPosition) / moveCooldown1;
                    }
                    break;
                case 2:
                    {
                        GetComponentInChildren<SpriteRenderer>().sprite = angry;
                        break;
                    }
            }
        }
        Vector2 playerPos = PlayerManager.player.battleAlien.transform.localPosition;
        Vector2 direction = (playerPos - (Vector2)transform.localPosition).normalized;

        if (Invincible && phaseTimer < Time.time)
        {
            Invincible = false;
            WallShot(180, 5, 0, 9f, 2);
            shotTimer1 = Time.time + shotCooldown1;
            shotTimer2 = Time.time + shotCooldown2;
        }
        if (phaseTimer < Time.time)
        {
            switch (phase)
            {
                case 0:
                    {
                        // Movement
                        if (moveTimer < Time.time)
                        {
                            moveTimer = Time.time + moveCooldown1;
                            Velocity = (positions[pos] - transform.localPosition) / moveCooldown1;
                            pos = (pos + 1) % 2;
                        }

                        // Attack
                        if (shotTimer1 < Time.time)
                        {
                            shotTimer1 = Time.time + shotCooldown1;
                            WallShot(20, 4, Random.Range(-3.9f, 3.9f));
                        }

                        if (shotTimer2 < Time.time)
                        {
                            shotTimer2 = Time.time + shotCooldown2;
                            Spawn(SPProj).GetComponent<Projectile>().InitialVelocity = direction * 4;
                        }
                        break;
                    }
                case 1:
                    {
                        if (moveTimer < Time.time)
                        {
                            moveTimer = Time.time + moveCooldown1;
                            Velocity = Vector3.zero;
                        }

                        // Attack
                        if (shotTimer1 < Time.time)
                        {
                            shotTimer1 = Time.time + 2f;
                            ProjWallShot();
                        }
                        break;
                    }
                case 2:
                    // Movement
                    float targetY = PlayerManager.player.battleAlien.transform.localPosition.y;
                    transform.localPosition = new Vector2(transform.localPosition.x, 
                        targetY * 0.1f + transform.localPosition.y * 0.9f);

                    // Attack
                    if (shotTimer1 < Time.time)
                    {
                        shotTimer1 = Time.time + shotCooldown1;
                        WallShot(30, 4f, Random.Range(-3.9f, 3.9f));
                    }

                    if (shotTimer2 < Time.time)
                    {
                        shotTimer2 = Time.time + shotCooldown2;
                        Spawn(SPProj).GetComponent<Projectile>().InitialVelocity = new Vector3(-4, 0);
                    }

                    break;
            }
        }
    }

    private void ProjWall()
    {
        for (int i = 0; i < wall.Length; i++)
        {
            if (wall[i] == null)
            {
                GameObject proj = Spawn(SNRProj);
                proj.transform.localPosition = new Vector2(-5.5f, 4 - 8f * i / (wall.Length - 1));
                proj.GetComponent<Projectile>().Freeze();
                wall[i] = proj;
            }
        }
    }

    private void ProjWallShot()
    {
        int x = Random.Range(0, 16);
        for (int i = 0; i < 20; i++)
        {
            GameObject proj = i > x && i < x + 5 ? Spawn(SPProj) : Spawn(SPNRProj);
            proj.transform.localPosition = new Vector2(5.5f, 4 - 8f * i / (wall.Length - 1));
            proj.GetComponent<Projectile>().InitialVelocity = new Vector2(-2f, 0);
            wall[i] = proj;
        }
    }

    // Make a wall that sends the player leftwards
    private void WallShot(float width, float height, float y, float dist = 10.5f, float life = 2.5f)
    {
        Projectile wall = Spawn(WProj).GetComponent<Projectile>();
        wall.InitialVelocity = new Vector3(-dist, 0) / life;
        wall.transform.localPosition = new Vector3(5.5f, y);
        wall.transform.localScale = new Vector3(width, height, 1);
        wall.SetLifespan(life);
    }
}
