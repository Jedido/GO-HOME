using UnityEngine;

public class BattleMoleKing2 : BattleMoleKing
{
    protected override int GetPhase()
    {
        return 1;
    }

    protected override GameObject GetProjectile()
    {
        return SNRProj;
    }
}
