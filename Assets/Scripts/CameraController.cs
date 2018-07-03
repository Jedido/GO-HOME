using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public float maxXBound, maxYBound;
    private float minXBound, minYBound;

	// Use this for initialization
	void Start () {
        Camera camera = GetComponent<Camera>();
        float horzExtent = camera.orthographicSize * Screen.width / Screen.height;
        maxXBound -= horzExtent;
        minXBound = horzExtent;
        float vertExtent = camera.orthographicSize;
        maxYBound -= vertExtent;
        minYBound = vertExtent;
        FixedUpdate();
        // gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
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
        if (py < minYBound)
        {
            py = minYBound;
        } else if (py > maxYBound)
        {
            py = maxYBound;
        }
        gameObject.transform.position = new Vector3(px, py, -10);
    }
}
