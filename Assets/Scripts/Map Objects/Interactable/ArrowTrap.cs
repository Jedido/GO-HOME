using System;
using UnityEngine;

public class ArrowTrap : MonoBehaviour, Interactable {
    private Animator animator;
    public GameObject arrow;
    public int direction;
    private Projectile a;

    void Start () {
        animator = GetComponent<Animator>();
        gameObject.AddComponent<Tripwire>().SetAngle(direction);
        a = Instantiate(arrow, transform.position, Quaternion.identity).GetComponent<Projectile>();
    }

    // TODO: make this the universal ArrowTrap
    public void SetDirection(int dir)
    {

    }

    public void Interact()
    {
        if (a != null)
        {
            animator.SetTrigger("Shoot");
            a.Velocity = new Vector2(0, -10);
            a = null;
        }
    }

    public void Reset()
    {
        if (a == null)
        {
            animator.SetTrigger("Reload");
            a = Instantiate(arrow, transform.position, Quaternion.identity).GetComponent<Projectile>();
        }
    }
}
