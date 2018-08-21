using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleClaw : Enemy {
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
        return (int)EnemyID.MoleClaw;
    }

    public override string GetName()
    {
        return "";
    }

    protected override void MakeInitial(int number)
    {
    }

    protected override void MakeBorder(int number)
    {
    }

    protected override Vector3 InitialPosition(int number = 1)
    {
        if (number == 2)
        {
            return new Vector2(3.5f, -2.5f);
        }
        return new Vector2(3.5f, 2.5f);
    }
}
