using UnityEngine;
using System.Collections;

public class SnapToGrid : MonoBehaviour {
    public float PPU = 20; // pixels per unit (your tile size)

    // Called after all other updates
    private void LateUpdate()
    {
        Vector3 position = transform.localPosition;

        position.x = (Mathf.Round(transform.position.x * PPU) / PPU);
        position.y = (Mathf.Round(transform.position.y * PPU) / PPU);

        transform.localPosition = position;
    }
}
