using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Design choice: make an "omniscript" for sword
public class Sword : Weapon {
    // There are 9 shops (9 upgrade spots), every other one will unlock an upgrade:
    //              shops 1, 3, 5, 7, 9
    // General upgrades [rank F-S]:
    //      Damage [1, 2, 3, 5, 8, 20] + Arc length [140, 150, 160, 170, 180, 225]
    //      Speed (swing time) [0.5, 0.45, 0.4, 0.35, 0.3, 0.2]
    //      Critical chance [10%, 20%, 30%, 40%, 50%, 80%]
    //      Critical: double sword speed and damage
    // 1-time powerups (when sword sprite changes):
    //      all rank D: walk while swinging
    //      all rank C: autoswing
    //      all rank B: directional swing
    //      all rank A: projectile
    private float rotation, startDegree, rVel;
    public Sprite[] swords;  // sword images
    public int power;
    public float arc, swingTime;

    // Use this for initialization
    void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {
        if (Active)
        {
            rotation += rVel;
            float angle = rotation < 0 ? 0 : rotation > arc ? arc : rotation;
            transform.localRotation = Quaternion.AngleAxis(angle + startDegree, Vector3.forward);
            if (rotation > arc)
            {
                Active = false;
            }
        }
    }

    public override void Attack(float degree)
    {
        startDegree = degree - arc / 2;
        rotation = 0;
        rVel = arc / swingTime / 60;  // assume ~60fps
        transform.localRotation = Quaternion.AngleAxis(startDegree, Vector3.forward);
        Active = true;
    }

    public override int GetDamage()
    {
        return power;
    }
}
