using System;
using UnityEngine;

// Destroys itself on Interaction
public class Destroyable : MonoBehaviour, Interactable
{
    public void Interact()
    {
        Camera.main.gameObject.GetComponent<CameraController>().shakeDuration = 0.1f;
        Destroy(gameObject);
    }

    public void Reset()
    {
    }
}
