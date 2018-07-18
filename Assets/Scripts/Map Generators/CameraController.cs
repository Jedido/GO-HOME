﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public float maxXBound, maxYBound;
    private float minXBound, minYBound;
    private float damp = 1f;

	// Use this for initialization
	void Start () {
        Camera camera = GetComponent<Camera>();
        if (maxXBound == 0)
        {
            // pretty much no border
            minXBound = minYBound = -10000;
            maxXBound = maxYBound = 10000;
        } else
        {
            float horzExtent = camera.orthographicSize * Screen.width / Screen.height;
            maxXBound -= horzExtent;
            minXBound = horzExtent;
            float vertExtent = camera.orthographicSize;
            maxYBound -= vertExtent;
            minYBound = vertExtent;
        }
    }

    // Update is called once per frame
    void LateUpdate () {
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
        gameObject.transform.position = new Vector3(
            gameObject.transform.position.x * (1 - damp) + px * damp, 
            gameObject.transform.position.y * (1 - damp) + py * damp, 
            -10);
    }
}
