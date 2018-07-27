using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basic Slime AI
// All slime will move like this
// Slime enemy ID is 0.
public class Slime : Enemy
{
    public override int GetID()
    {
        return (int)EnemyID.Slime;
    }

    protected override void AddThreats()
    {
        Debug.Log("Add stuff");
    }
}
