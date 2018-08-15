using UnityEngine;

public class Spike : MonoBehaviour, Interactable {
    public Sprite[] sprites;
    private SpriteRenderer sprite;
    private bool active, spike;
    private static readonly float spikeWarning = 0.5f;
    private static readonly float spikeTime = 1f;
    private float spikeTimer;

    protected void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (active && spikeTimer < Time.time)
        {
            active = false;
            spike = false;
            sprite.sprite = sprites[0];
        } else if (spike && spikeTimer < Time.time)
        {
            spikeTimer = Time.time + spikeTime;
            active = true;
            sprite.sprite = sprites[2];
        }
    }

    public void Interact()
    {
        if (!spike)
        {
            spikeTimer = Time.time + spikeWarning;
            spike = true;
            sprite.sprite = sprites[1];
        }
    }

    public void Reset()
    {
        if (spike)
        {
            spike = false;
            sprite.sprite = sprites[0];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && collision.tag.Equals("Player"))
        {
            collision.GetComponent<PlayerController>().Hit(1, true);
        }
    }
}
