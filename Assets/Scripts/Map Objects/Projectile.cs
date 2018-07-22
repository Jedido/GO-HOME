using UnityEngine;

public class Projectile : MonoBehaviour {
    private Rigidbody2D rb2d;
    public Vector2 Velocity
    {
        set { rb2d.velocity = value; }
    }

	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            collision.GetComponent<PlayerController>().Hit(1, true);
            Destroy(gameObject);
        }
        else if (collision.tag.Equals("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
