using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Movement
    public float speed;  // TODO: eventually use PlayerManager.player.speed and remove this variable
    private Rigidbody2D rb2d;
    private Animator animator;
    private GameObject portal;

    // Attack
    public Weapon primary;
    private bool attacking;
    private Weapon live = null;

	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool fire1 = Input.GetMouseButtonDown(0);
        bool fire2 = Input.GetMouseButtonDown(1);

        if (fire2 && portal)
        {
            transform.position = portal.transform.position;
            portal = null;
        }

        float ms = speed;
        if (horizontal != 0 && vertical != 0)
        {
            // Moving diagonally
            ms *= Mathf.Sqrt(2) / 2f;
        }

        if (fire1 && !live)
        {
            // TODO: use player's equipped weapon
            attacking = true;
            live = Instantiate(primary);
            live.SetPlayer(this.gameObject);
        }

        rb2d.velocity = new Vector2(horizontal * ms, vertical * ms);
    }

    // Set animations
    private void LateUpdate()
    {
        /*
        // TODO change this to direction integer when other animations added
        if (rb2d.velocity.y < 0)
        {
            animator.SetBool("Walking Down", true);
        } else
        {
            animator.SetBool("Walking Down", false);
        }
        animator.SetFloat("Speed", rb2d.velocity.magnitude / 5f);
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Portal"))
        {
            Portal script = collision.gameObject.GetComponent<Portal>();
            portal = script.portal;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Portal"))
        {
            portal = null;
        }
    }
}
