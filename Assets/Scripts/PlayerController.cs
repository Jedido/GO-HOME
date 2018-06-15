using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed;  // TODO: eventually use PlayerManager.player.speed and remove this variable
    private Rigidbody2D rb2d;
    private Animator animator;

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
        // TODO change this to direction integer when other animations added
        if (rb2d.velocity.y < 0)
        {
            animator.SetBool("Walking Down", true);
        } else
        {
            animator.SetBool("Walking Down", false);
        }
        animator.SetFloat("Speed", rb2d.velocity.magnitude / 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.position.y > collision.transform.position.y) return;
        transform.position -= new Vector3(0, 0, 1);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (transform.position.z == 0 || transform.position.y > collision.transform.position.y) return;
        transform.position += new Vector3(0, 0, 1);
    }
}
