using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public int minXBound, maxXBound, yBound;

	// Use this for initialization
	void Start () {
        gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float px = player.transform.position.x;
        float py = player.transform.position.y;
        if (px < minXBound)
        {
            px = minXBound;
        } else if (px > maxXBound)
        {
            px = maxXBound;
        }
        if (py < yBound)
        {
            py = yBound;
        }
        gameObject.transform.position = new Vector3(px * 0.3f + gameObject.transform.position.x * 0.7f, py * 0.3f + gameObject.transform.position.y * 0.7f, -10);
    }
}
