using UnityEngine;

public class GreenSlime : Slime
{
    private Vector2 initial;

    protected override Vector3 InitialPosition(int number = 0)
    {
        if (initial.x != 0 && initial.y != 0)
        {
            return initial;
        }

        do {
            initial = new Vector2(Random.Range(-5, 6), Random.Range(-3, 4));
        } while (initial.x == 0 && initial.y == 0);
        return initial;
    }

    protected override void MakeInitial(int number)
    {
        base.MakeInitial(number);

        // Place a block on starting location
        AddBlock(InitialPosition().x, initial.y, 15, 15);
    }

    public override int GetID()
    {
        return 2;
    }

    public override string GetName()
    {
        return "Green Slime";
    }

    public override void Die()
    {
        base.Die();
        AddLoot((int)Item.Type.Green, 1);
    }
}
