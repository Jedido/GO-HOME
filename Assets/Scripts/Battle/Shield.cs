using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {
    private Vector3 dir;
    public Vector3 Direction
    {
        set {
            dir = value;
            float angle = Vector3.Angle(Vector3.up, dir);
            if (dir.x > 0)
            {
                angle = -angle;
            }
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Projectile"))
        {
            EnemyProjectile old = collision.gameObject.GetComponent<EnemyProjectile>();
            if (old.Reflect)
            {
                Vector2 original = old.Velocity;
                float magnitude = original.magnitude;
                Projectile proj = collision.gameObject.AddComponent<PlayerProjectile>();
                proj.SetAttributes(old);
                Destroy(old);

                // Version 1 (opposite direction)
                // proj.InitialVelocity = -original;

                // Version 2 (shield direction)
                // proj.Velocity = dir * magnitude;

                // Version 3 (physics reflection)
                proj.InitialVelocity = (Vector3)original + 2 * Vector3.Project(original, dir).magnitude * dir.normalized
                    + Vector3.Project(PlayerManager.player.battleAlien.GetComponent<PlayerBattleController>().Velocity, dir);

            }
        }
    }
}
