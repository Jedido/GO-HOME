using UnityEngine;

// All the generators might be refactored under one superclass 
public class GeneratePlains : MonoBehaviour
{
    // Probably change into a radius
    public int width;
    public int height;

    // Value between 0 and 1
    // higher the value, less blocks are spawned
    public float threshold;

    // Higher the value, more compact noise
    public float scaleY;
    public float scaleX;

    // Randomized in order to sample the perlin noise from a different area each time
    private float offsetX;
    private float offsetY;

    public GameObject block;  // 1x1 block
    public GameObject bigBlock; // 2x2 block

    void Start()
    {
        offsetX = Random.Range(0, 9999f);
        offsetY = Random.Range(0, 9999f);
        GenerateWalls();
        GenerateTerrain();
    }

    // Make borders
    void GenerateWalls()
    {
        for (float i = 0.5f; i <= width + 0.5f; i += 2)
        {
            Instantiate(bigBlock, new Vector3(i, height + 0.5f), Quaternion.identity);
            Instantiate(bigBlock, new Vector3(i, 0.5f), Quaternion.identity);
        }

        for (float i = 2.5f; i < height; i += 2)
        {
            Instantiate(bigBlock, new Vector3(width + 0.5f, i), Quaternion.identity);
            Instantiate(bigBlock, new Vector3(0.5f, i), Quaternion.identity);
        }
    }

    void GenerateTerrain()
    {
        // TODO detect larger rectangles, make them into bigger blocks (see GenerateWalls for help)
        // TODO add exit (one of the other 3 corners) and make sure it is reachable (percolate)
        // These tasks will require multiple passes through the map (an array is highly recommended)
        bool[,] blocks = new bool[width, height];
        for (int x = 2; x < width - 1; x++)
        {
            for (int y = 2; y < height - 1; y++)
            {
                // This is where the player starts
                if (x < 15 && y < 15) continue;

                float xCoord = (float)x / width * scaleX + offsetX;
                float yCoord = (float)y / height * scaleY + offsetY;

                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                blocks[x, y] = sample > threshold;
            }
        }

        // Place trees
        for (int x = 2; x < width - 1; x++)
        {
            for (int y = height - 2; y >= 2; y--)
            {
                if (blocks[x, y] && blocks[x + 1, y] && blocks[x, y - 1] && blocks[x + 1, y - 1])
                {
                    Instantiate(bigBlock, new Vector3(x + 0.5f, y - 0.5f, 0), Quaternion.identity);
                    blocks[x, y] = false;  // do not place anything here anymore
                    blocks[x, y - 1] = false;  // do not place anything here anymore
                    blocks[x + 1, y] = false;  // do not place anything here anymore
                    blocks[x + 1, y - 1] = false;  // do not place anything here anymore
                }
            }
        }

        for (int x = 2; x < width - 1; x++)
        {
            for (int y = 2; y < height - 1; y++)
            {
                if (blocks[x, y])
                {
                    Instantiate(block, new Vector3(x, y, 0), Quaternion.identity);
                    blocks[x, y] = false;
                }
            }
        }
    }
}
