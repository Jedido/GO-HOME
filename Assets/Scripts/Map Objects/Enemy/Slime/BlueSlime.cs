public class BlueSlime : Slime
{
    public override int GetID()
    {
        return 1;
    }

    public override string GetName()
    {
        return "Blue Slime";
    }

    public override void Die()
    {
        base.Die();
        AddLoot((int)Item.Type.Blue, 1);
    }
}
