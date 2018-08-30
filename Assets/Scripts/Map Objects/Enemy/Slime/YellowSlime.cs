using System;

public class YellowSlime : Slime
{
    public override int GetID()
    {
        return 3;
    }

    public override string GetName()
    {
        return "Yellow Slime";
    }

    public override void Die()
    {
        base.Die();
        AddLoot((int)Item.Type.Yellow, 1);
    }
}
