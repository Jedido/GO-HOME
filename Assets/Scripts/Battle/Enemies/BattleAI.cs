using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// In charge of movement and attacks during battle phase
public abstract class BattleAI : MonoBehaviour {
    private int health;
    private Enemy e;

    private void Start()
    {
        health = GetHealth();
    }

    protected abstract int GetHealth();

    public void SetEnemy(Enemy enemy)
    {
        e = enemy;
    }
	
    public void Hit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            e.Die();
        }
    }
}
