using UnityEngine;
using System.Collections;

// Weapon superclass.
public class Weapon : MonoBehaviour {

    private float swing;
    private GameObject player;

	// Use this for initialization
	void Start () {
        // begin attack
        swing = 0;
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.AngleAxis(swing, Vector3.forward);
        transform.position = player.transform.position + new Vector3(0, 1, 0);
        if (swing >= 90)
        {
            // end swing
            Destroy(gameObject, 0.05f);
        } else
        {
            swing += 10;
        }
	}

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: deal damage
    }
}
