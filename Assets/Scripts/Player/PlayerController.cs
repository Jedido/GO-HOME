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
        // Check if in pause
        if (PlayerManager.player.CanMove())
        {
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


            rb2d.velocity = new Vector2(horizontal * ms, vertical * ms);
        }
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
