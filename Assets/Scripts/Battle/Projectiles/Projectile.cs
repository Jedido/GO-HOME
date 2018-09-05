using UnityEngine;

public abstract class Projectile : MonoBehaviour {
    private Rigidbody2D rb2d;
    public float lifespan; // lifespan, in seconds
    private bool freeze, used;
    private Vector2 v;
    public bool phase;
    public bool Phase
    {
        set { phase = value; }
    }
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
        get { return rb2d.velocity; }
        set {
            Vector2 dir = value;
            float angle = Vector3.Angle(Vector3.up, dir);
            if (dir.x > 0)
            {
                angle = -angle;
            }
            transform.rotation = Quaternion.Euler(0, 0, angle + 180);
            rb2d.velocity = value;
        }
    }

	protected void Start () {
        if (rb2d == null)
        {
            rb2d = GetComponent<Rigidbody2D>();
        }
        if (lifespan == 0)
        {
            SetLifespan(5);
        }
        Velocity = v;
	}

    // Match the attributes of the other projectile.
    // Kind of like a copy constructor.
    public void SetAttributes(Projectile other)
    {
        phase = other.phase;
        used = other.used;
        freeze = other.freeze;
        v = other.v;
        lifespan = other.lifespan;
        rb2d = other.rb2d;
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

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    protected void Update()
    {
        if (!freeze && lifespan < Time.time)
        {
            Die();
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (!used && !phase)
        {
            if (collision.tag.Equals("Wall"))
            {
                Die();
            }
        }
    }
}
