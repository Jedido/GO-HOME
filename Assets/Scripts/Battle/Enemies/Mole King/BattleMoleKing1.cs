using UnityEngine;

// Phase 1 of mole king
// Phase shots
public class BattleMoleKing1 : BattleMoleKing
{
    protected override GameObject GetProjectile()
    {
        return SPProj;
    }
}
