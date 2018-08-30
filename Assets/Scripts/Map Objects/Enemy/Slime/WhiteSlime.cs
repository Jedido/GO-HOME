using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteSlime : Slime
{
    protected override Vector3 InitialPosition(int number = 0)
    {
        return BattleWhiteSlime.corners[0];
    }

    public override int GetID()
    {
        return 4;
    }

    public override string GetName()
    {
        return "White Slime";
    }

    protected override void MakeInitial(int number)
    {
        disableInit = true;
        /*
        AddBlock(1, 1, 5, 5);
        AddBlock(1, -1, 5, 5);
        AddBlock(-1, -1, 5, 5);
        AddBlock(-1, 1, 5, 5);
        */
        AddBlock(0, 2, 40, 5);
        AddBlock(2, 0, 5, 40);
        AddBlock(0, -2, 40, 5);
        AddBlock(-2, 0, 5, 40);
        AddBlock(-5.25f, 0, 30, 180);
        AddBlock(5.25f, 0, 30, 180);
    }

    public override void Die()
    {
        base.Die();
        AddLoot((int)Item.Type.Gold, Random.Range(10, 21));
        AddLoot((int)Item.Type.Yellow, Random.Range(3, 6));
        AddLoot((int)Item.Type.Blue, Random.Range(3, 6));
        AddLoot((int)Item.Type.Green, Random.Range(3, 6));
        AddLoot((int)Item.Type.Red, Random.Range(3, 6));
    }
}
