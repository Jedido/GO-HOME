﻿using UnityEngine;

public class GreenSlime : Slime
{
    private Vector2 initial;

    protected override Vector3 InitialPosition()
    {
        while (initial == null || (initial.x == 0 && initial.y == 0))
        {
            initial = new Vector2(Random.Range(-5, 6), Random.Range(-3, 4));
        }
        return initial;
    }

    protected override void MakeInitial(int number)
    {
        base.MakeInitial(number);

        // Place a block on starting location
        AddBlock(InitialPosition().x, initial.y, 10, 10);
    }

    public override int GetID()
    {
        return 2;
    }
}