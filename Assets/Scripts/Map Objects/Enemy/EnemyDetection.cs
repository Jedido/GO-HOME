using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour {
    private bool disabled;
    public float detectionRadius, maxRadius, encounterRadius;
    public float DetectionRadius
    {
        set { detectionRadius = value; }
    }
    public float MaxRadius
    {
        set { maxRadius = value; }
    }
    public float EncounterRadius
    {
        set { encounterRadius = value; }
    }
    private Enemy enemy;
    private SpriteRenderer sprite;
    private Color color;
    private float waitTimer, waitTime;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        sprite = GetComponent<SpriteRenderer>();
        waitTime = 0;
    }

    // Update is called once per frame
    void Update () {
        if (PlayerManager.player.CanMove())
        {
            if (waitTime != 0)
            {
                waitTimer = Time.time + waitTime;
                waitTime = 0;
            }

            if (!disabled)
            {
                float distance = Vector3.Distance(PlayerManager.player.alien.transform.position, transform.position + new Vector3(0.5f, 0.5f));
                if (distance < encounterRadius)
                {
                    // Enter fight
                    enemy.BecomeAggro();
                    enemy.InitBattle();
                }
                else if (distance < detectionRadius)
                {
                    // Become active
                    enemy.BecomeActive();
                }
                else if (distance > maxRadius)
                {
                    // Become inactive
                    enemy.BecomeInactive();
                    // DisableDetection(0.5f, false);  // Reduce the number of calculations
                }
            }
            else if (waitTimer < Time.time)
            {
                disabled = false;
                sprite.color = color;
            }
        }
    }

    // No Engaging until time is up
    public void DisableDetection(float time, bool fade = true)
    {
        disabled = true;
        waitTime = Time.time + time;
        color = sprite.color;
        if (fade)
        {
            sprite.color = new Color(color.r, color.g, color.b, 0.5f);
        }
    }
}
