using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMoleKing : BattleCPU {
    public Sprite[] phases;
    public GameObject body, left, right;

    // Use this for initialization
    new void Start()
    {
        base.Start();
    }

    protected override int GetHealth()
    {
        return 10;
    }

    protected override void UpdateCPU()
    {
    }

    public override void Hit(int damage)
    {
        if (damage >= HP)
        {
            // TODO: run away
            Invincible = true;
            Velocity = (transform.localPosition - new Vector3(0, 0)).normalized * 3;
        } else
        {
            base.Hit(damage);
        }
    }
}
