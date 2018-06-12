using UnityEngine;

public class GenerateMountains : MonoBehaviour
{

    public int width;
    public int height;

    // Higher the value, more compact noise
    public float scaleX;
    public float scaleCloudX;
    public float scaleCloudY;
    public float scaleCaveX;
    public float scaleCaveY;

    // Randomized in order to sample the perlin noise from a different area each time
    public float offsetX;
    public float offsetY;
    public float offsetCaveX;
    public float offsetCaveY;

    // Higher the value, less cloud generated
    public float cloudThreshold;
    public float caveThreshold;
    public int baseHeight;

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
            bool[] mapY = new bool[height * 3];

            // Make mountains
            float xCoord = (float)x / width * scaleX + offsetX;

            float sample = Mathf.PerlinNoise(xCoord, offsetY);

            for (int i = 0; i < sample * sample * height * (1.3f - Mathf.Abs((float)x - width / 2) / width) + baseHeight; i++)
            {
                mapY[i] = true;
            }

            // Make clouds
            for (int y = height + baseHeight; y < height * 1.7f + baseHeight; y++)
            {
                float xCoord2 = (float)x / width * scaleCloudX + offsetX;
                float yCoord2 = (float)y / height * scaleCloudY + offsetY;

                float sample2 = Mathf.PerlinNoise(xCoord2, yCoord2);
                if (sample / 2 + sample2 < cloudThreshold + (float)(y - baseHeight) / height / 4 - (1 - Mathf.Abs((float)x - width / 2) / width) / 3)
                {
                    Instantiate(block, new Vector3(x - width / 2, y - height / 2, 0), Quaternion.identity);
                }
            }

            // Make caves
            for (int y = 0; y < baseHeight + height / 4; y++)
            {
                float xCoord2 = (float)x / width * scaleCloudX + offsetX;
                float yCoord2 = (float)y / height * scaleCloudY + offsetY;

                float sample2 = Mathf.PerlinNoise(xCoord2, yCoord2);
                if (sample2 < caveThreshold - (float)y / height / 3 * 2)
                {
                    mapY[y] = false;
                }
            }

            // Generate line
            for (int y = 0; y < height * 3; y++)
            {
                if (mapY[y])
                {
                    Instantiate(block, new Vector3(x - width / 2, y, 0), Quaternion.identity);
                }
            }
        }
    }
}
