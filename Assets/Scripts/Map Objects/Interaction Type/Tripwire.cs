using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Detects the player in a direction and fires when the player is there
public class Tripwire : InteractionType {
    private int dir;
    public enum Direction { DOWN, RIGHT, UP, LEFT };

    public void SetAngle(int direction)
    {
        dir = direction;
    }

	// Update is called once per frame
	void Update () {
        Vector3 direction = dir == (int)Direction.RIGHT ? new Vector3(1, 0) 
            : dir == (int)Direction.UP ? new Vector3(0, 1)
            : dir == (int)Direction.LEFT ? new Vector3(-1, 0) 
            : new Vector3(0, -1);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
        if (hit.collider && hit.collider.tag.Equals("Player"))
        {
            Activate();
            // enabled = false;
        }
    }
}
