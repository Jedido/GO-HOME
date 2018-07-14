using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Design choice: make an "omniscript" for sword
public class Sword : Weapon {
    // There are 9 shops (9 upgrade spots)
    // General upgrades [rank 1-10]:
    //      Damage [1, 3, 6, 10, 15, 21, 28, 36, 45, 55]
    //      Speed (swingTime) [0.75, 0.7, 0.65, 0.6, 0.55, 0.5, 0.45, 0.4, 0.35, 0.3, 0.25]
    //      Arc size [135, 144, 153, 162, 171, 180, 189, 198, 207, 216, 225]
    // 1-time powerups (when sword sprite changes):
    //      all rank 2: autoswing
    //      all rank 3: walk while swinging
    //      all rank 5: directional swing
    //      all rank 8: projectile (damage)
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
