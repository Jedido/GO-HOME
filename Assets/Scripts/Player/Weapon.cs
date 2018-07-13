using UnityEngine;
using System.Collections;

// Weapon superclass.
public abstract class Weapon : MonoBehaviour {
    private int damage;
    public int ATK
    {
        get { return damage; }
    }

    private SpriteRenderer sr;
    private bool active;
    public bool Active
    {
        get { return active; }
        protected set { sr.enabled = value; active = value; }
    }

	// Use this for initialization
	protected void Init() {
        damage = GetDamage();
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
	}

    public abstract void Attack(float degree);
    public abstract int GetDamage();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().Hit(damage);
        }
    }
}
