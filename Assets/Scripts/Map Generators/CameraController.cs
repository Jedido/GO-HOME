using UnityEngine;
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
    }

    // Update is called once per frame
    void Update () {
        float px = PlayerManager.player.alien.transform.position.x;
        float py = PlayerManager.player.alien.transform.position.y;
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
        gameObject.transform.position = new Vector3(
            gameObject.transform.position.x * (1 - damp) + px * damp,
            gameObject.transform.position.y * (1 - damp) + py * damp,
            -10);
    }
}
