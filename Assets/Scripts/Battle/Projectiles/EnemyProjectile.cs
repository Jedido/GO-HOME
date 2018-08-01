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
        if (!Hit)
        {
            if (collision.tag.Equals("Player"))
            {
                PlayerManager.player.alien.GetComponent<PlayerController>().Hit(1, true);
                Hit = true;
                Destroy(gameObject);
            }
        }
    }
}
