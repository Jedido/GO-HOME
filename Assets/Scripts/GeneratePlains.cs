using UnityEngine;

public class GeneratePlains : MonoBehaviour
{

    public int width;
    public int height;

    // Higher the value, more compact noise
    public float scaleX;

    // Randomized in order to sample the perlin noise from a different area each time
    public float offsetX;
    public float offsetY;

    public GameObject block;

    void Start()
    {
        offsetX = Random.Range(0, 9999f);
        offsetY = Random.Range(0, 9999f);
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        for (int x = 0; x < width; x++)
        {
            float xCoord = (float)x / width * scaleX + offsetX;

            float sample = Mathf.PerlinNoise(xCoord, offsetY);

            for (int i = 0; i < sample * height; i++)
            {
                Instantiate(block, new Vector3(x - width / 2, i, 0), Quaternion.identity);
            }
        }
    }
}
