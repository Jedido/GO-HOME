using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleController : MonoBehaviour {
    private float speed;
    private Rigidbody2D rb2d;
    public float projTimer, projCooldown, projSpeed;
    private GameObject projectile;

	// Use this for initialization
	void Start () {
        speed = 4f;
        rb2d = GetComponent<Rigidbody2D>();
        projectile = SpriteLibrary.library.PlayerProjectile;
        PlayerManager.player.battleAlien = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerManager.player.battle.GetComponent<BattleController>().Active)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            bool fire1 = Input.GetMouseButton(0);

            // Change speed if going diagonally
            float ms = speed;
            if (horizontal != 0 && vertical != 0)
            {
                // Moving diagonally
                ms *= Mathf.Sqrt(2) / 2f;
            }

            // Shooting
            if (fire1 && projTimer < Time.time)
            {
                Vector2 pos = (Vector2)Input.mousePosition / 40f - new Vector2(8, 6);
                Vector2 dir = pos - (Vector2)transform.localPosition;
                GameObject p = Instantiate(projectile, transform, true);
                p.GetComponent<Projectile>().InitialVelocity = dir.normalized * projSpeed;
                p.transform.localPosition = new Vector3(0, 0);
                projTimer = Time.time + projCooldown;
            }

            rb2d.velocity = new Vector2(horizontal * ms, vertical * ms);
        }
    }
}
