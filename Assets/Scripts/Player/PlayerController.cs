﻿using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Movement
    public float speed;  // TODO: eventually use PlayerManager.player.speed and remove this variable
    private Rigidbody2D rb2d;
    private Animator animator;
    private Interactable interactable;
    private int direction;
    private enum Direction { UP, LEFT, DOWN, RIGHT, NONE };

    // Attack
    // public GameObject primary;
    // private Weapon weapon1;
    private bool moving;

    // Camera
    // public Camera cam;

	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        PlayerManager.player.alien = gameObject;
        // weapon1 = primary.GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool fire1 = Input.GetMouseButtonDown(0);
        bool fire2 = Input.GetMouseButtonDown(1);

        if (fire2 && interactable != null)
        {
            interactable.Interact();
        }

        // Set direction
        moving = true;
        if (vertical > 0)
        {
            direction = (int)Direction.UP;
        }
        else if (vertical < 0)
        {
            direction = (int)Direction.DOWN;
        }
        else if (horizontal > 0)
        {
            direction = (int)Direction.RIGHT;
        }
        else if (horizontal < 0)
        {
            direction = (int)Direction.LEFT;
        }
        else
        {
            direction = (int)Direction.NONE;
            moving = false;
        }

        // Change speed if going diagonally
        float ms = speed;
        if (horizontal != 0 && vertical != 0)
        {
            // Moving diagonally
            ms *= Mathf.Sqrt(2) / 2f;
        }

        /*
        if (fire1 && !attacking)
        {
            Vector3 triangle = Input.mousePosition - cam.WorldToScreenPoint(transform.position);
            float angle = (Mathf.Atan(triangle.y / triangle.x) * 180 / Mathf.PI + 360) % 360;
            if (triangle.x < 0)
            {
                angle += 180;
                angle %= 360;
            }

            // Attacking direction
            // TODO: calculate these based on the screen res (since the screen isn't square)
            if (angle < 45)
            {
                direction = (int)Direction.RIGHT;
            }
            else if (angle < 135)
            {
                direction = (int)Direction.UP;
            }
            else if (angle < 225)
            {
                direction = (int)Direction.LEFT;
            }
            else if (angle < 315)
            {
                direction = (int)Direction.DOWN;
            }
            else
            {
                direction = (int)Direction.RIGHT;
            }

            animator.SetInteger("Direction", direction);
            attacking = true;
            weapon1.Attack(angle - 90);
        } else if (!weapon1.Active)
        {
            attacking = false;
        }
        */

        rb2d.velocity = new Vector2(horizontal * ms, vertical * ms);
    }

    // Set animations
    private void LateUpdate()
    {
        if (direction != (int)Direction.NONE)
        {
            animator.SetInteger("Direction", direction);
        }
        animator.SetBool("Moving", moving);

        // animator.SetBool("Attacking", attacking);

        // TODO: update PlayerManager
        PlayerManager.player.Position = transform.position;
    }

    public void Hit(int damage, bool trueDamage)
    {
        if (!trueDamage)
        {
            damage -= PlayerManager.player.GetPlayerStat((int)PlayerManager.PlayerStats.DEFENSE);
            damage = damage < 1 ? 1 : damage;
        }
        PlayerManager.player.SetHealth(PlayerManager.player.GetPlayerStat((int)PlayerManager.PlayerStats.HP)
            - damage);

        // TODO: set invincibility frames
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Interactable"))
        {
            interactable = collision.gameObject.GetComponent<Interactable>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (interactable == null && collision.tag.Equals("Interactable"))
        {
            interactable = collision.gameObject.GetComponent<Interactable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (interactable != null
            && collision.tag.Equals("Interactable") 
            && interactable.Equals(collision.gameObject.GetComponent<Interactable>()))
        {
            interactable = null;
        }
    }
}
