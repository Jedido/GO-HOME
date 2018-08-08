using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleController : MonoBehaviour {
    private float speed;
    private Rigidbody2D rb2d;
    public float projTimer, projCooldown, projSpeed;
    public float hitTimer, hitCooldown;  // move this out

    // Weapon 1 (default)
    private GameObject projectile;
    private Shield shield;

    // Weapon 2 (shovel?)


    private Animator animator;

    public Vector3 Velocity
    {
        get { return rb2d.velocity; }
    }

	// Use this for initialization
	void Start () {
        speed = 4f;
        rb2d = GetComponent<Rigidbody2D>();
        projectile = SpriteLibrary.library.PlayerProjectile;
        PlayerManager.player.battleAlien = gameObject;
        shield = transform.GetChild(0).gameObject.GetComponent<Shield>();
        shield.gameObject.SetActive(false);
        animator = GetComponent<Animator>();
        hitCooldown = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerManager.player.battle.GetComponent<BattleController>().Active)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            bool fire1 = Input.GetMouseButton(0);
            bool fire2 = Input.GetMouseButton(1);

            // Change speed if going diagonally
            float ms = speed;
            if (horizontal != 0 && vertical != 0)
            {
                // Moving diagonally
                ms *= Mathf.Sqrt(2) / 2f;
            }

            // Shielding
            if (fire2)
            {
                shield.gameObject.SetActive(true);
                Vector2 pos = (Vector2)Input.mousePosition / 80f - new Vector2(8, 6);
                shield.Direction = pos - (Vector2)transform.localPosition;
            }
            else
            {
                shield.gameObject.SetActive(false);

                // Shooting
                if (fire1 && projTimer < Time.time)
                {
                    Vector2 pos = (Vector2)Input.mousePosition / 80f - new Vector2(8, 6);
                    Vector2 dir = pos - (Vector2)transform.localPosition;
                    GameObject p = Instantiate(projectile, transform, true);
                    p.GetComponent<Projectile>().InitialVelocity = dir.normalized * projSpeed;
                    p.transform.localPosition = new Vector3(0, 0);
                    projTimer = Time.time + projCooldown;
                }
            }

            if (hitTimer < Time.time)
            {
                animator.SetBool("Invincible", false);
            }

            if (!shield.gameObject.activeSelf)
            {
                rb2d.velocity = new Vector2(horizontal * ms, vertical * ms);
            } else
            {
                rb2d.velocity = new Vector2(horizontal * ms, vertical * ms) * 0.5f;
            }
        }
    }

    public void Hit(int damage)
    {
        if (hitTimer < Time.time)
        {
            hitTimer = hitCooldown + Time.time;
            damage -= PlayerManager.player.GetPlayerStat((int)PlayerManager.PlayerStats.DEFENSE);
            damage = damage < 1 ? 1 : damage;
            PlayerManager.player.SetHealth(PlayerManager.player.GetPlayerStat((int)PlayerManager.PlayerStats.HP)
                - damage);

            // Hit Animation
            animator.SetBool("Invincible", true);
            PlayerManager.player.battle.GetComponent<BattleController>().HitEffect();
        }
    }
}
