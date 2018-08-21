using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMoleClaw : BattleCPU {
    private Vector3 origin;
    public Vector3 Origin
    {
        set { origin = value; }
    }

    private static readonly float raiseCooldown = 0.5f;
    private float raiseTimer;
    private bool raise;

    new protected void Start()
    {
        base.Start();
        transform.localPosition = origin;
    }

    public void Invert()
    {
        raise = false;
    }

    protected override int GetHealth()
    {
        return -1;
    }

    protected override void UpdateCPU()
    {
        if (raiseCooldown < Time.time)
        {
            raise = !raise;
            if (raise)
            {
                Debug.Log(Time.time);
                Velocity = new Vector2(0, 3);
            }
            else
            {
                Velocity = new Vector2(0, -3);
            }
        }
    }
}