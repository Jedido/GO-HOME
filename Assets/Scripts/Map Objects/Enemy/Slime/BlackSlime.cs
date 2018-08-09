﻿using UnityEngine;

public class BlackSlime : Slime
{
    protected override Vector3 InitialPosition(int number = 0)
    {
        return new Vector3(4, 0);
    }

    public override int GetID()
    {
        return 5;
    }

    public override string GetName()
    {
        return "Black Slime";
    }

    protected override void MakeInitial(int number)
    {
        disableInit = true;
        for (int i = -4; i <= 4; i++)
        {
            AddBlock(-3.5f, i, 5, 5);
            AddBlock(3.5f, i, 5, 5);
        }
    }
}
