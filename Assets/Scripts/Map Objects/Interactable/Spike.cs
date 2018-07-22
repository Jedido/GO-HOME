using UnityEngine;

public class Spike : MonoBehaviour, Interactable {
    private Animator animator;
    public bool active;

    protected void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void Interact()
    {
        animator.SetTrigger("Spike");
    }

    public void Reset()
    {
        animator.ResetTrigger("Spike");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && collision.tag.Equals("Player"))
        {
            collision.GetComponent<PlayerController>().Hit(1, true);
        }
    }
}
