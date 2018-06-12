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

    public GameObject block;

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
        GameObject wallN = (GameObject)Instantiate(block, new Vector3(width / 2, height), Quaternion.identity);
        wallN.transform.localScale = new Vector2(width + 1, 1);

        GameObject wallE = (GameObject)Instantiate(block, new Vector3(0, height / 2), Quaternion.identity);
        wallE.transform.localScale = new Vector2(1, height - 1);

        GameObject wallW = (GameObject)Instantiate(block, new Vector3(width, height / 2), Quaternion.identity);
        wallW.transform.localScale = new Vector2(1, height - 1);

        GameObject wallS = (GameObject)Instantiate(block, new Vector3(width / 2, 0), Quaternion.identity);
        wallS.transform.localScale = new Vector2(width + 1, 1);
    }

    void GenerateTerrain()
    {
        // TODO detect larger rectangles, make them into bigger blocks (see GenerateWalls for help)
        // TODO add exit (one of the other 3 corners) and make sure it is reachable (percolate)
        // These tasks will require multiple passes through the map (an array is highly recommended)
        for (int x = 1; x < width; x++)
        {
            for (int y = 1; y < height; y++)
            {
                // This is where the player starts
                if (x < 15 && y < 15) continue;

                float xCoord = (float)x / width * scaleX + offsetX;
                float yCoord = (float)y / height * scaleY + offsetY;

                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                if (sample > threshold)
                {
                    Instantiate(block, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }
    }
}
