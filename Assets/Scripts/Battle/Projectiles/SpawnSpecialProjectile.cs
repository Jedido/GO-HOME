using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpecialProjectile : Projectile {
    public GameObject digSpot;
    private Enemy mole;

    public void SetMole(Enemy o)
    {
        mole = o;
    }

    private void OnDestroy()
    {
        Instantiate(digSpot, transform.position, Quaternion.identity).GetComponent<Encounter>().enemy = mole;
    }
}
