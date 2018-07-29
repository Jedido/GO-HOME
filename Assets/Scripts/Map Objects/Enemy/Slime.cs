using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basic Slime AI
// Slime enemy ID is 0.
public class Slime : Enemy
{
    private Animator animator;
    private bool active;

    new private void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override void BecomeActive()
    {
        if (!active)
        {
            active = true;
            animator.SetTrigger("Wake");
        }
    }

    public override void BecomeInactive()
    {
        if (active)
        {
            active = false;
            animator.SetTrigger("Sleep");
        }
    }

    public override int GetID()
    {
        return (int)EnemyID.Slime;
    }

    protected override void MakeInitial()
    {
        // Randomly add 10-20 blocks on a 11x7 grid
        float blocks = Random.Range(10, 20);
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
}
