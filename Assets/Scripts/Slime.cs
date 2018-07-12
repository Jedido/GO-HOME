using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {
    // TODO: make enemy superclass with some of these fields
    // TODO: make Slime into an abstract class (inherited by different slimes)
    public int damage;
    public float movespeed;
    public float jumpDelay, jumpTimer, jumpRange, jumpTime;
    public float detectionRadius;  // when enemy is alerted of player presence
    private bool found = false;
    private bool jumping, jump;
    public Vector3 jumpDir;

    private Animator anim;
    private Rigidbody2D rb2d;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
	void Update () {
        Vector3 dir = new Vector3(PlayerManager.player.Position.x - transform.position.x, 
            PlayerManager.player.Position.y - transform.position.y);
        float dist = dir.magnitude;
        if (dist < detectionRadius)
        {
            found = true;
        }
        if (found)
        {
            if (!jump && jumpTimer > Time.time)
            {
                rb2d.velocity = new Vector3();
            } else if (jump && jumpTimer < Time.time)
            {
                jump = false;
            } else if (jumping && jumpTimer < Time.time)
            {
                jumping = false;
                jumpTimer = jumpTime + Time.time;
                jumpDir = dir.normalized;
                jump = true;
            } else if (!jump && dist < jumpRange)
            {
                jumping = true;
                jumpTimer = jumpDelay + Time.time;
            } else if (jump)
            {
                rb2d.velocity = jumpDir * movespeed * 2 * (0.5f + jumpTimer - Time.time);
            } else
            {
                rb2d.velocity = dir.normalized * movespeed;
            }
        }
    }

    private void LateUpdate()
    {
        anim.SetBool("Jump", jump);
        anim.SetBool("Jumping", jumping);
    }
}
