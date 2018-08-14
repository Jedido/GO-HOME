using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleKing : Enemy {
    public GameObject mole;

    private GameObject[,] spikes;
    private static readonly int WIDTH = 15;
    private static readonly int HEIGHT = 15;

    private static readonly float spikeCooldown = 1f;
    private static readonly float moveCooldown = 0.25f;
    private static readonly float burrowCooldown = 2f;
    private float spikeTimer, moveTimer, burrowTimer;
    private bool burrowed;

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
        spikes = new GameObject[WIDTH + 1, HEIGHT];
        for (int i = 0; i < WIDTH + 1; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                if ((j != 0 && j != HEIGHT - 1) || (i > 1 && i < WIDTH - 1))
                {
                    GameObject s = Instantiate(spike, spikeHolder.transform, true);
                    s.transform.localPosition = new Vector2(i, j);
                    spikes[i, j] = s;
                }
            }
        }
        spikeHolder.transform.parent = transform;
        spikeHolder.transform.position = transform.position - new Vector3(0.525f, 0);
    }

    private void Update()
    {
        if (burrowTimer < Time.time)
        {
            burrowTimer = Time.time + burrowCooldown;
            if (burrowed)
            {
                burrowed = false;
                animator.SetTrigger("Unburrow");
                mole.transform.localPosition = corners[Random.Range(0, 4)];
            } else
            {
                burrowed = true;
                animator.SetTrigger("Burrow");
            }
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
        return 10;
    }

    public override string GetName()
    {
        return "Mole King";
    }

    protected override void MakeInitial(int number)
    {

    }

    protected override void MakeBorder(int number)
    {
        // No Border
    }
}
