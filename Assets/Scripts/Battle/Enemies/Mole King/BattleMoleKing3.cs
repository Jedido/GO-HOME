using UnityEngine;

public class BattleMoleKing3 : BattleMoleKing
{
    protected override GameObject GetProjectile()
    {
        return SPNRProj;
    }

    public override void Hit(int damage)
    {
        base.Hit(damage);
    }
}
