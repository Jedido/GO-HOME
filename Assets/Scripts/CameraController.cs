using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public int maxXBound, maxYBound;

	// Use this for initialization
	void Start () {
        gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        // float horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float px = player.transform.position.x;
        float py = player.transform.position.y;
        if (px < 0)
        {
            px = 0;
        } else if (px > maxXBound)
        {
            px = maxXBound;
        }
        if (py < 0)
        {
            py = 0;
        } else if (py > maxYBound)
        {
            py = maxYBound;
        }
        gameObject.transform.position = new Vector3(px * 0.1f + gameObject.transform.position.x * 0.9f, py * 0.1f + gameObject.transform.position.y * 0.9f, -10);
    }
}
