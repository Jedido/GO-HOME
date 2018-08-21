using System;
using UnityEngine;

// Phase 1 of mole king
// Phase shots
public class BattleMoleKing1 : BattleMoleKing
{
    protected override GameObject GetProjectile()
    {
        return SPProj;
    }

    protected override int GetPhase()
    {
        return 0;
    }
}
