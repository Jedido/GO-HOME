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
        Reload();
    }

    private void Reload()
    {
        a = Instantiate(arrow, transform.position, Quaternion.identity).GetComponent<Projectile>();
        a.Freeze();
        SetDirection(direction);
    }

    // TODO: make this the universal ArrowTrap
    public void SetDirection(int dir)
    {
        Quaternion orient = Quaternion.AngleAxis(dir * 90, Vector3.forward);
        transform.rotation = orient;
        a.gameObject.transform.rotation = orient;
    }

    public void Interact()
    {
        if (a != null)
        {
            switch (direction)
            {
                case 0: a.Velocity = new Vector2(0, -20); break;
                case 1: a.Velocity = new Vector2(20, 0); break;
                case 2: a.Velocity = new Vector2(0, 20);  break;
                case 3: a.Velocity = new Vector2(-20, 0); break;
            }
            a.Unfreeze();
            a = null;
            animator.SetTrigger("Shoot");
        }
    }

    public void Reset()
    {
        if (a == null)
        {
            animator.SetTrigger("Reload");
            Reload();
        }
    }
}
