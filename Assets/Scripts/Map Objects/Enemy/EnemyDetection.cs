using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour {
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

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update () {
        float distance = Vector3.Distance(PlayerManager.player.alien.transform.position, transform.position);
        if (distance < encounterRadius)
        {
            // Enter fight
            enemy.BecomeAggro();
            enemy.InitBattle();
        } else if (distance < detectionRadius)
        {
            // Become active
            enemy.BecomeActive();
        } else if (distance > maxRadius)
        {
            // Become inactive
            enemy.BecomeInactive();
        }

    }
}
