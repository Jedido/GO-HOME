using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed;  // TODO: eventually use PlayerManager.player.speed and remove this variable
    private Rigidbody2D rb2d;

    // Attack
    public Weapon primary;
    private bool attacking;
    private Weapon live = null;

	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
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
}
