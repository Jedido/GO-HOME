using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleKing : Enemy {
    public GameObject mole;
    public GameObject gray;
    public GameObject brown;

    private Spike[,] spikes;
    private static readonly int WIDTH = 15;
    private static readonly int HEIGHT = 15;

    private Vector2 spikeDest;
    private static readonly float spikeCooldown = 0.4f;
    private static readonly float throwCooldown = 1f;
    private static readonly float burrowCooldown = 2f;
    private float spikeTimer, throwTimer, burrowTimer;
    private int hitCount, phase;
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

        GameObject spikeHolder = new GameObject();
        GameObject spike = SpriteLibrary.library.SpikeTrap;
        spikes = new Spike[WIDTH + 1, HEIGHT];
        for (int i = 0; i < WIDTH + 1; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                if ((j != 0 && j != HEIGHT - 1) || (i > 1 && i < WIDTH - 1))
                {
                    GameObject s = Instantiate(spike, spikeHolder.transform, true);
                    s.transform.localPosition = new Vector2(i, j);
                    spikes[i, j] = s.GetComponent<Spike>();
                }
            }
        }
        spikeHolder.transform.parent = transform;
        spikeHolder.transform.position = transform.position - new Vector3(0.525f, 0);
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
                hitCount = 0;
                spikeDest = pos;
            }
        } else if (burrowTimer < Time.time)
        {
            burrowed = true;
        }

        // Above ground attack
        if (!burrowed && !done)
        {
            if (throwTimer < Time.time)
            {
                throwTimer = Time.time + throwCooldown / phase;
                hitCount++;
                if (hitCount > phase * 10)
                {
                    done = true;
                    hitCount = 0;
                }
                else
                {
                    GameObject obj;
                    if (hitCount == phase * 7)
                    {
                        obj = Instantiate(brown, mole.transform.position, Quaternion.identity);
                        obj.GetComponent<SpawnSpecialProjectile>().SetMole(this);
                    }
                    else
                    {
                        obj = Instantiate(gray, mole.transform.position, Quaternion.identity);
                    }
                    Projectile proj = obj.GetComponent<Projectile>();
                    Vector3 dest = (Vector3)pos - mole.transform.localPosition + (Vector3)Random.insideUnitCircle * 3 / phase;
                    proj.InitialVelocity = dest / 3 * phase;
                    proj.SetLifespan(3 / (float)phase);
                }
            }
        }

        // Below ground attack
        if (burrowed)
        {
            if (spikeTimer < Time.time)
            {
                spikeTimer = Time.time + spikeCooldown / phase;
                Vector3 spikeDir = (Vector3)spikeDest - mole.transform.localPosition;
                if (spikeDir.magnitude > 2)
                {
                    spikeDir = spikeDir.normalized * 2;
                }
                mole.transform.localPosition += spikeDir;
                hitCount++;
                int x = (int)Mathf.Round(mole.transform.localPosition.x);
                int y = (int)Mathf.Round(mole.transform.localPosition.y + 0.15f) - 1;
                Spike(x, y);
                Spike(x + 1, y);
                Spike(x, y + 1);
                Spike(x - 1, y);
                Spike(x, y - 1);
                Spike(x + 1, y + 1);
                Spike(x - 1, y + 1);
                Spike(x + 1, y - 1);
                Spike(x - 1, y - 1);
            }

            if (hitCount != 0)
            {
                if (hitCount < 10)
                {
                    spikeDest = pos;
                }
                else if (hitCount == 10)
                {
                    spikeDest = corners[Random.Range(0, 4)];
                }
                else if ((spikeDest - (Vector2)mole.transform.localPosition).magnitude < 0.05f)
                {
                    arrived = true;
                    hitCount = 0;
                }
            }
        }
    }

    private void Spike(int x, int y)
    {
        if (x >= 0 && x < WIDTH + 1 && y >= 0 && y < HEIGHT && spikes[x, y] != null)
        {
            spikes[x, y].Interact();
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
        if (burrowed && (mole.transform.position - PlayerManager.player.alien.transform.position).magnitude < 5f)
        {
            base.InitBattle(number, disableBlocks, false);
            hitCount = 9;
        } else
        {
            PlayerManager.player.Alert("Nothing found underground!", Color.white, 3);
        }
    }

    protected override void MakeInitial(int number)
    {

    }

    protected override void MakeBorder(int number)
    {
        // No Border
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
