using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile {

    new protected void Start()
    {
        base.Start();
    }

    new protected void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
        if (!Hit)
        {
            if (collision.tag.Equals("Enemy"))
            {
                collision.GetComponent<BattleCPU>().Hit(1);
                Hit = true;
                Destroy(gameObject);
            }
        }
    }
}
