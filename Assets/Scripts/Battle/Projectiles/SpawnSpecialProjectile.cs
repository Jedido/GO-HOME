using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpecialProjectile : Projectile {
    private MoleKing mole;

    public void SetMole(MoleKing o)
    {
        mole = o;
    }

    private void OnDestroy()
    {
        mole.SetBurrow(transform.localPosition);
    }
}
