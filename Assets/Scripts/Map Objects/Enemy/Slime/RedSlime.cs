using System;

public class RedSlime : Slime
{
    public override int GetID()
    {
        return 0;
    }

    public override string GetName()
    {
        return "Red Slime";
    }

    public override void Die()
    {
        base.Die();
        AddLoot((int)Item.Type.Red, 1);
    }
}
