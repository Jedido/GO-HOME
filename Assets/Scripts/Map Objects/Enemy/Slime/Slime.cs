using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basic Slime AI
// Slime enemy ID is 0.
public abstract class Slime : Enemy
{
    private SpriteRenderer sprite;
    public Sprite[] sprites;
    public Color c;
    private bool active;

    new private void Start()
    {
        base.Start();
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = sprites[0];
        sprite.color = c;
    }

    public override void BecomeAggro()
    {
        if (active)
        {
            sprite.sprite = sprites[2];
        }
    }

    public override void BecomeActive()
    {
        if (!active)
        {
            active = true;
            sprite.sprite = sprites[1];
        }
    }

    public override void BecomeInactive()
    {
        if (active)
        {
            active = false;
            sprite.sprite = sprites[0];
        }
    }

    protected override void MakeInitial(int number)
    {
        // Randomly add 10-20 blocks on a 11x7 grid
        float blocks = Random.Range(10, 20) / number;
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
