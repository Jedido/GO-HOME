using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteSlime : Slime
{
    protected override Vector3 InitialPosition()
    {
        return BattleWhiteSlime.corners[Random.Range(0, 4)];
    }

    public override int GetID()
    {
        return 4;
    }

    protected override void MakeInitial(int number)
    {

    }
}
