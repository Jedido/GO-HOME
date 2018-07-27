using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleController : MonoBehaviour {
    private float speed;
    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        speed = 4f;
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Change speed if going diagonally
        float ms = speed;
        if (horizontal != 0 && vertical != 0)
        {
            // Moving diagonally
            ms *= Mathf.Sqrt(2) / 2f;
        }

        rb2d.velocity = new Vector2(horizontal * ms, vertical * ms);
    }

    public void Hit(int damage)
    {
        damage -= PlayerManager.player.GetPlayerStat((int)PlayerManager.PlayerStats.DEFENSE);
        damage = damage < 1 ? 1 : damage;
        PlayerManager.player.SetHealth(PlayerManager.player.GetPlayerStat((int)PlayerManager.PlayerStats.HP)
            - damage);
    }
}
