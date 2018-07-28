using UnityEngine;

public abstract class Projectile : MonoBehaviour {
    private Rigidbody2D rb2d;
    private float lifespan; // lifespan, in seconds
    private Vector2 v;
    public Vector2 InitialVelocity
    {
        set { v = value; }
    }
    public Vector2 Velocity
    {
        set { rb2d.velocity = value; }
    }

	protected void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        SetLifespan(10);
        rb2d.velocity = v;
	}

    public void SetLifespan(float time)
    {
        lifespan = Time.time + time;
    }

    protected void Update()
    {
        if (lifespan < Time.time)
        {
            Destroy(gameObject);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
