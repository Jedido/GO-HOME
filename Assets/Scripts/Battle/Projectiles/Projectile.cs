using UnityEngine;

public abstract class Projectile : MonoBehaviour {
    private Rigidbody2D rb2d;
    public float lifespan; // lifespan, in seconds
    private bool freeze, used;
    private Vector2 v;
    public bool Hit
    {
        get { return used; }
        set { used = value; }
    }
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
        freeze = false;
        rb2d.velocity = v;
	}

    public void SetLifespan(float time)
    {
        lifespan = Time.time + time;
    }

    public void Freeze()
    {
        freeze = true;
        lifespan -= Time.time;
    }

    public void Unfreeze()
    {
        freeze = false;
        lifespan += Time.time;
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
        if (!used)
        {
            if (collision.tag.Equals("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }
}
