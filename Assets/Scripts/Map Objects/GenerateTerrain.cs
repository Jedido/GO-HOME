using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{

    public int width;
    public int height;

    // Higher the value, more compact noise
    public float scaleX;
    public float scaleY;

    // Randomized in order to sample the perlin noise from a different area each time
    public float offsetX;
    public float offsetY;

    // Higher the value, less terrain generated
    public float threshold;

    public GameObject block;

    void Start()
    {
        offsetX = Random.Range(0, 9999f);
        offsetY = Random.Range(0, 9999f);
        generateBlocks();
    }

    void generateBlocks()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CalculateBlock(x, y);
            }
        }
    }

    void CalculateBlock(int x, int y)
    {
        float xCoord = (float)x / width * scaleX + offsetX;
        float yCoord = (float)y / height * scaleY + offsetY;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);

        if (sample > threshold)
        {
            Instantiate(block, new Vector3(x - width / 2, y - height / 2, 0), Quaternion.identity);
        }
    }
}
