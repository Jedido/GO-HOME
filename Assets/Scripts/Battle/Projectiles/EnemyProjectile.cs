using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile {
    private bool reflect;
    public bool Reflect
    {
        get { return reflect; }
        set { reflect = value; }
    }

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
                if (PlayerManager.player.battle.activeSelf)
                {
                    PlayerManager.player.battleAlien.GetComponent<PlayerBattleController>().Hit(1);
                    Hit = true;
                    Destroy(gameObject);
                }
                else
                {
                    PlayerManager.player.alien.GetComponent<PlayerController>().Hit(1, true);
                    Hit = true;
                    Destroy(gameObject);
                }
            }
        }
    }
}
