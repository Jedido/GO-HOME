using UnityEngine;

public class GenerateCave: MonoBehaviour
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
        bool[,] map = new bool[width, height];

        // Calculate blocks with perlin noise over a threshold
        for (int x = 0; x < width; x++)
        {
            float xCoord = ((float)x / width - 0.5f) * scaleX + offsetX;
            for (int y = 3; y < height - 3; y++)
            {
                float yCoord = ((float)y / height - 0.5f) * scaleY + offsetY;

                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                if (sample > threshold - Mathf.Abs((float)y - (float)height / 7 * 3) / height / 4)
                {
                    map[x, y] = true;
                }
            }
        }

        // Create borders
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                map[x, y] = true;
            }
            for (int y = height - 3; y < height; y++)
            {
                map[x, y] = true;
            }
        }

        // Create a starting point
        for (int x = 10; x < 40; x++)
        {
            for (int y = 3; y < 5; y++)
            {
                map[x, y] = true;
            }
            for (int y = 5; y < 25; y++)
            {
                map[x, y] = false;
            }
        }

        // Fill in unreachable areas
        bool[,] percolateMap = new bool[width, height];
        // Percolate(map, percolateMap, 15, 10);

        // Make blocks
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y])
                {
                    Instantiate(block, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }
    }
}
