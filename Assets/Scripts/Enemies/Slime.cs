using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basic Slime AI
// All slime will move like this
// Slime enemy ID is 0.
public class Slime : Enemy {
    public float jumpDelay, jumpRadius, jumpTime, restTime;
    private float jumpTimer;
    private bool found = false;
    private bool jumping, jump;
    public Vector3 jumpDir;
    public GameObject eyes;
    public Sprite[] eyeTypes;  // 0 is normal, 1 is sleeping
    private float wakeup;  // speed ramps up after waking up

    private Animator anim;
    private SpriteRenderer eyeSprite;

    private void Start()
    {
        Init();
        anim = GetComponentInChildren<Animator>();
        eyeSprite = eyes.GetComponent<SpriteRenderer>();
        wakeup = 0f;
    }

    // Update is called once per frame
	new void Update () {
        Vector3 dir = new Vector3(PlayerManager.player.Position.x - transform.position.x, 
            PlayerManager.player.Position.y - transform.position.y);
        float dist = dir.magnitude;
        if (!found && Active)
        {
            found = true;
            eyeSprite.sprite = eyeTypes[0];
        }
        if (found)
        {
            if (wakeup < 1)
            {
                wakeup += 0.1f;
            } else if (wakeup != 1)
            {
                wakeup = 1;
            }
            // Set eye direction
            float newX = 0;
            float newY = 0;
            if (dir.x > 0.5f)
            {
                newX = 0.05f;
            } else if (dir.x < -0.5f)
            {
                newX = -0.05f;
            }
            if (dir.y > 0.5f)
            {
                newY = 0.05f;
            } else if (dir.y < -0.5f)
            {
                newY = -0.05f;
            }
            eyes.transform.localPosition = new Vector3(newX, newY, 0);

            // Do movement
            if (!jump && jumpTimer > Time.time)
            {
                SetDirection(new Vector3());  // no movement
            } else if (jump && jumpTimer < Time.time)
            {
                // Rest
                jump = false;
                jumpTimer = restTime + Time.time;
                eyeSprite.sprite = eyeTypes[0];
            }
            else if (jumping && jumpTimer < Time.time)
            {
                // Jump!
                jumping = false;
                jumpTimer = jumpTime + Time.time;
                jumpDir = dir.normalized;
                jump = true;
            }
            else if (!jump && dist < jumpRadius)
            {
                // In range to jump
                jumping = true;
                jumpTimer = jumpDelay + Time.time;
            }
            else if (jump)
            {
                SetDirection(jumpDir * MS * 2 * (0.5f + jumpTimer - Time.time));
            } else
            {
                SetDirection(dir.normalized * MS * wakeup);
            }
        }

        base.Update();
    }

    private void LateUpdate()
    {
        anim.SetBool("Jump", jump);
        anim.SetBool("Jumping", jumping);
        anim.SetFloat("Speed", (GetSpeed() + 1) / 2);
    }

    public override int GetID()
    {
        return 0;
    }

    protected override int GetATK()
    {
        return 1;
    }

    protected override int GetHP()
    {
        return 5;
    }

    protected override float GetMS()
    {
        return 1.5f;
    }

    protected override float GetKnockback()
    {
        return 4f;
    }

    protected override float GetRadius()
    {
        return 5f;
    }

    protected override void Die()
    {
        // TODO: drop gold and stuff
    }
}
