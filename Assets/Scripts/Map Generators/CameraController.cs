﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    // Camera shake credited to ftvs.

    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    private void LateUpdate()
    {
        Vector3 originalPos = camTransform.localPosition;
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }

    public float maxXBound, maxYBound;
    private float minXBound, minYBound;
    private GameObject center;
    private bool centering;
    private float damp = 1f;

	// Use this for initialization
	void Start () {
        camTransform = gameObject.transform;
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
        center = PlayerManager.player.alien;
        gameObject.transform.position = new Vector3(center.transform.position.x, center.transform.position.y, -10);
    }

    // Update is called once per frame
    void Update () {
        if (center == null)
        {
            center = PlayerManager.player.alien;
            centering = false;
            damp = 1;
        }

        float px = center.transform.position.x;
        float py = center.transform.position.y;
        if (px < minXBound)
        {
            px = minXBound;
        }
        else if (px > maxXBound)
        {
            px = maxXBound;
        }
        if (py < minYBound)
        {
            py = minYBound;
        }
        else if (py > maxYBound)
        {
            py = maxYBound;
        }
        if (centering || !PlayerManager.player.battle.activeSelf)
        {
            gameObject.transform.position = new Vector3(
                gameObject.transform.position.x * (1 - damp) + px * damp,
                gameObject.transform.position.y * (1 - damp) + py * damp,
                -10);
        }
    }

    public void CenterOn(GameObject obj)
    {
        center = obj;
        centering = true;
        damp = 0.5f;
    }
}
