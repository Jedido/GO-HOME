using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleKing : Enemy {
    public GameObject mole;
    public GameObject brown, block, burrow;

    private MoleDirtBlock[,] ground;
    private static readonly int WIDTH = 15;
    private static readonly int HEIGHT = 15;

    private Vector2Int curPos;
    private static readonly float spikeCooldown = 10f;
    private static readonly float moveCooldown = 0.5f;
    private static readonly float burrowCooldown = 2f;
    private float spikeTimer, moveTimer, throwTimer, burrowTimer;
    private int phase, corner;
    private bool burrowed, arrived, done;

    private Animator animator;

    private static readonly Vector2[] corners = { new Vector2(0, 0),
        new Vector2(WIDTH, 0),
        new Vector2(0, HEIGHT),
        new Vector2(WIDTH, HEIGHT) };

    // Use this for initialization
    new void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();

        GameObject blockHolder = new GameObject();
        ground = new MoleDirtBlock[WIDTH + 1, HEIGHT];
        for (int i = 0; i < WIDTH + 1; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                if ((j != 0 && j != HEIGHT - 1) || (i > 1 && i < WIDTH - 1))
                {
                    GameObject s = Instantiate(block, blockHolder.transform, true);
                    s.transform.localPosition = new Vector2(i, j);
                    ground[i, j] = s.GetComponent<MoleDirtBlock>();
                }
            }
        }
        blockHolder.transform.parent = transform;
        blockHolder.transform.position = transform.position - new Vector3(0.525f, 0);
        arrived = true;
        burrowed = false;
        phase = 1;
    }

    protected override void UpdateEnemy()
    {
        Vector2 pos = PlayerManager.player.alien.transform.position - transform.position;

        // Burrow/Unburrow transitions
        if (arrived && burrowTimer < Time.time)
        {
            burrowTimer = Time.time + burrowCooldown;
            if (burrowed)
            {
                burrowed = false;
                done = false;
                animator.SetTrigger("Unburrow");
            } else if (done)
            {
                arrived = false;
                animator.SetTrigger("Burrow");
                animator.ResetTrigger("Unburrow");
                spikeTimer = Time.time + spikeCooldown;
                corner = Random.Range(0, 4);
            }
        } else if (burrowTimer < Time.time)
        {
            burrowed = true;
        }

        // Above ground attack
        if (!burrowed && !done)
        {
            GameObject obj = Instantiate(brown, mole.transform.position, Quaternion.identity);
            obj.transform.parent = transform;
            obj.GetComponent<SpawnSpecialProjectile>().SetMole(this);
            Projectile proj = obj.GetComponent<Projectile>();
            Vector3 dest = new Vector3(Random.Range(2, WIDTH - 2), Random.Range(2, HEIGHT - 2) + 1);
            proj.InitialVelocity = dest.normalized * 5;
            proj.SetLifespan(dest.magnitude / 5);
            done = true;
        }

        // Below ground attack
        if (burrowed && moveTimer < Time.time)
        {
            moveTimer = Time.time + moveCooldown;
            Vector2 dest;
            if (spikeTimer > Time.time)
            {
                dest = pos;
            } else
            {
                dest = corners[corner];
            }
            Vector2 dir = dest - new Vector2(mole.transform.localPosition.x, mole.transform.localPosition.y);
            if (dir.magnitude > 1f)
            {
                dir = dir.normalized * 1f;
            }
            mole.transform.localPosition += (Vector3)dir;
            Vector2Int newPos = new Vector2Int(Mathf.RoundToInt(mole.transform.localPosition.x), Mathf.RoundToInt(mole.transform.localPosition.y));
            Spike(newPos);
            if (Vector3.Distance(corners[corner], mole.transform.localPosition) < 0.5f)
            {
                mole.transform.localPosition = corners[corner];
                arrived = true;
            }
        }
    }

    public void Spike(Vector2Int v)
    {
        if (!v.Equals(curPos))
        {
            //ToggleArea(curPos.x, curPos.y, false);
            ToggleArea(v.x, v.y, true);
            curPos = v;
        }
    }

    private void ToggleArea(int x, int y, bool set)
    {
        ToggleSpike(x, y, set);
        ToggleSpike(x + 1, y, set);
        ToggleSpike(x, y + 1, set);
        ToggleSpike(x - 1, y, set);
        ToggleSpike(x, y - 1, set);
        ToggleSpike(x + 1, y + 1, set);
        ToggleSpike(x + 1, y - 1, set);
        ToggleSpike(x - 1, y + 1, set);
        ToggleSpike(x - 1, y - 1, set);
    }

    private void ToggleSpike(int x, int y, bool set)
    {
        if (x >= 0 && x < WIDTH + 1 && y >= 0 && y < HEIGHT && ground[x, y] != null)
        {
            ground[x, y].Spike = set;
        }
    }

    public void SetBurrow(Vector2 pos)
    {
        GameObject dig = Instantiate(burrow, transform, false);
        dig.GetComponent<Encounter>().enemy = this;
        dig.transform.localPosition = pos; 
    }

    public void SetConfig()
    {
        switch (phase)
        {
            case 1: break;
            case 2: break;
            case 3: break;
        }
    }

    // Always active
    public override void BecomeActive()
    {
    }

    public override void BecomeAggro()
    {
    }

    public override void BecomeInactive()
    {
    }

    public override int GetID()
    {
        return (int)EnemyID.MoleKing + phase - 1;
    }

    public override string GetName()
    {
        return "Mole King";
    }

    public override void InitBattle(int number = 1, bool disableBlocks = false, bool center = true)
    {
        if (burrowed && (mole.transform.position - PlayerManager.player.alien.transform.position).magnitude < 10f)
        {
            base.InitBattle(number, disableBlocks, false);
            mole.transform.position = corners[Random.Range(0, 4)];
        } else
        {
            PlayerManager.player.Alert("Nothing found underground!", Color.white, 3);
        }
    }

    protected override void MakeInitial(int number)
    {
        // Randomly add 7-10 blocks on a 11x7 grid
        float blocks = Random.Range(7, 10) / number;
        int left = 77;
        for (int i = -5; i < 6; i++)
        {
            for (int j = -3; j < 4; j++)
            {
                if (left != 0 && blocks != 0 && Random.value < blocks / left)
                {
                    AddBlock(i, j, 10, 10);
                    blocks--;
                }
                left--;
            }
        }
    }

    public override void Hide()
    {
        Destroy(battleForm);
        if (phase == 3)
        {
            Destroy(gameObject);
        } else
        {
            phase++;
        }
    }
}
