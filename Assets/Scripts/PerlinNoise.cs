using UnityEngine;

public class PerlinNoise : MonoBehaviour {

    private int width = 256;
    private int height = 256;
    private Renderer render;

    // Higher the value, more compact noise
    public float scaleX;
    public float scaleY;

    // Randomized in order to sample the perlin noise from a different area each time
    public float offsetX;
    public float offsetY;

	void Start ()
    {
        offsetX = Random.Range(0, 9999f);
        offsetY = Random.Range(0, 9999f);
        render = GetComponent<Renderer>();
        render.material.mainTexture = generateTexture();
	}

    void Update()
    {
        render.material.mainTexture = generateTexture();
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

    // Perlin noise returns a value between 0 and 1.
    // The higher the value, the whiter the pixel at (x, y)
    Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scaleX + offsetX;
        float yCoord = (float)y / height * scaleY + offsetY;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }
}
