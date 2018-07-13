using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon {
    private float rotation, startDegree;
    public float startVel, rVel, rAcc;

    // Use this for initialization
    void Start () {
        Init();
        startVel = 10;
        rVel = 0;
        rAcc = 1;
	}
	
	// Update is called once per frame
	void Update () {
        if (Active)
        {
            rotation += rVel;
            if (rotation < 135)
            {
                rVel += rAcc;
            } else if (rVel > startVel)
            {
                rVel -= rAcc * 2;
            } else
            {
                rVel = startVel;
            }
            float angle = rotation < 0 ? 0 : rotation > 180 ? 180 : rotation;
            transform.localRotation = Quaternion.AngleAxis(angle + startDegree, Vector3.forward);
            if (rotation > 270)
            {
                Active = false;
            }
        }
    }

    public override void Attack(float degree)
    {
        startDegree = degree - 90;
        rotation = -20;
        rVel = startVel;
        transform.localRotation = Quaternion.AngleAxis(startDegree, Vector3.forward);
        Active = true;
    }

    public override int GetDamage()
    {
        return 2;
    }
}
