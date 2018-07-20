using System;
using UnityEngine;

public class Spike : PressurePlate {
    private Animator animator;
    public bool active;
    public float spikeDuration; // how long the spikes stay up

    protected void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public override void Interact()
    {
        animator.SetTrigger("Spike");
    }

    public override void Reset()
    {
        animator.ResetTrigger("Spike");
    }

    new private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (active && collision.tag.Equals("Player"))
        {
            collision.GetComponent<PlayerController>().Hit(1, true);
        }
    }
}
