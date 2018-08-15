using UnityEngine;

public class ArrowTrap : MonoBehaviour, Interactable {
    public Sprite[] sprites;
    public GameObject arrow;
    public int direction;
    private Projectile a;
    private SpriteRenderer sprite;

    void Start () {
        sprite = GetComponent<SpriteRenderer>();
        if (direction == -1)
        {
            direction = Random.Range(0, 4);
        }
        Reload();
    }

    private void Reload()
    {
        sprite.sprite = sprites[0];
        a = Instantiate(arrow, transform.position, Quaternion.identity).GetComponent<Projectile>();
        a.SetLifespan(5);
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
            sprite.sprite = sprites[1];
        }
    }

    public void Reset()
    {
        if (a == null)
        {
            Reload();
        }
    }
}
