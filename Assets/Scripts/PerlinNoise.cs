using UnityEngine;

public class PerlinNoise : MonoBehaviour {

    public int width = 256;
    public int height = 256;
    private Renderer renderer;

    // Higher the value, more compact noise
    public float scaleX = 5f;
    public float scaleY = 20f;

    // Randomized in order to sample the perlin noise from a different area each time
    public float offsetX = 100f;
    public float offsetY = 100f;

	void Start ()
    {
        offsetX = Random.Range(0, 9999f);
        offsetY = Random.Range(0, 9999f);
        renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = generateTexture();
	}

    private void Update()
    {
        renderer.material.mainTexture = generateTexture();
    } 

    Texture2D generateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scaleX + offsetX;
        float yCoord = (float)y / height * scaleY + offsetY;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }
}
