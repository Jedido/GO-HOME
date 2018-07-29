using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile {

    new protected void Start()
    {
        base.Start();
    }

    new protected void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.tag.Equals("Battle Player"))
        {
            collision.GetComponent<PlayerBattleController>().Hit(1);
            Destroy(gameObject);
        }
    }
}
