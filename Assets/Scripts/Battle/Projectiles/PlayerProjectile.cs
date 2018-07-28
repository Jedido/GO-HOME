using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile {

    new protected void Start()
    {
        base.Start();
    }

    new protected void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.tag.Equals("Enemy"))
        {
            collision.GetComponent<BattleAI>().Hit(1);
            Destroy(gameObject);
        }
    }
}
