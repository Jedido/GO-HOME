using UnityEngine;
using UnityEngine.Tilemaps;

// All the generators might be refactored under one superclass 
public class GeneratePlains : MonoBehaviour
{
    // width and height of map 
    public int width, height;

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
    public GameObject bg1, bg2, bg3; // ground tile
    public Tilemap background, obstacles;
    public Tile[] tiles;
    // Tile layout
    // 0-8: from UL to BR
    // 9-12: Inverted UL, UR, BL, BR
    // 13: center 2
    // 14: Bush
    public Tile bush
    {
        get { return tiles[14]; }
    }
    // 15: Tree
    public Tile tree
    {
        get { return tiles[15]; }
    }

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
        for (int i = 2; i < width - 2; i += 2)
        {
            obstacles.SetTile(new Vector3Int(i, 0, 0), tree);
            obstacles.SetTile(new Vector3Int(i, height - 2, 0), tree);
        }
        for (int i = 0; i < height; i += 2)
        {
            obstacles.SetTile(new Vector3Int(0, i, 0), tree);
            obstacles.SetTile(new Vector3Int(width - 2, i, 0), tree);
        }
    }

    void GenerateTerrain()
    {
        // TODO detect larger rectangles, make them into bigger blocks (see GenerateWalls for help)
        // TODO add exit (one of the other 3 corners) and make sure it is reachable (percolate)
        // These tasks will require multiple passes through the map (an array is highly recommended)
        bool[,] blocks = new bool[width, height];
        int[,] density = new int[width + 1, height + 1]; // keeps track of how many obstacles within 2 spaces
        // calculate density from borders
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                density[i, j] = 5;
                density[i, height - 1 - j] = 5;
                density[j, i] = 5;
                density[width - 1 - j, i] = 5;
            }
            density[i, 3]++;
            density[3, i]++;
            density[i, width - 4]++;
            density[height - 4, i]++;
        }

        for (int x = 2; x < width - 2; x++)
        {
            for (int y = 2; y < height - 2; y++)
            {
                // This is where the player starts
                if (x < 15 && y < 15) continue;

                float xCoord = (float)x / width * scaleX + offsetX;
                float yCoord = (float)y / height * scaleY + offsetY;

                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                bool res = sample > threshold;
                blocks[x, y] = res;
                if (res)
                {
                    // increment all spaces within 2 of (x, y)
                    for (int i = x - 1; i <= x + 1; i++)
                    {
                        for (int j = y - 1; j <= y + 1; j++)
                        {
                            if (i > 0 && i < width && j > 0 && j < width)
                            {
                                density[i, j]++;
                            }
                        }
                    }
                    density[x - 2, y]++;
                    density[x, y - 2]++;
                    density[x + 2, y]++;
                    density[x, y + 2]++;
                }
            }
        }

        // Background
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (density[x, y] == 0)
                {
                    background.SetTile(new Vector3Int(x, y, 1), tiles[13]);
                } else
                {
                    if (x > 2 && y > 2 && x < width - 2 && y < width - 2)
                    {
                        bool u = density[x, y + 1] != 0;
                        bool b = density[x, y - 1] != 0;
                        bool l = density[x - 1, y] != 0;
                        bool r = density[x + 1, y] != 0;
                        bool ul = density[x - 1, y + 1] == 0;
                        bool ur = density[x + 1, y + 1] == 0;
                        bool bl = density[x - 1, y - 1] == 0;
                        bool br = density[x + 1, y - 1] == 0;
                        Tile place = null;
                        if (u && b && l && r)
                        {
                            if (ul && ur)
                            {
                                place = tiles[1];
                            } else if (bl && br)
                            {
                                place = tiles[7];
                            } else if (bl && ul)
                            {
                                place = tiles[3];
                            } else if (br && ur)
                            {
                                place = tiles[5];
                            } else if (ul)
                            {
                                place = tiles[12];
                            } else if (ur)
                            {
                                place = tiles[11];
                            } else if (bl)
                            {
                                place = tiles[10];
                            } else if (br)
                            {
                                place = tiles[9];
                            } else
                            {
                                place = tiles[4];
                            }
                        } else if (b && l && r && !bl && !br)
                        {
                            place = tiles[1];
                        } else if (u && l && r && !ul && !ur)
                        {
                            place = tiles[7];
                        } else if (u && b && l && !bl && !ul)
                        {
                            place = tiles[5];
                        } else if (u && b && r && !br && !ur)
                        {
                            place = tiles[3];
                        } else if (u && r && !ur)
                        {
                            place = tiles[6];
                        } else if (u && l && !ul)
                        {
                            place = tiles[8];
                        } else if (b && r && !br)
                        {
                            place = tiles[0];
                        } else if (b && l && !bl)
                        {
                            place = tiles[2];
                        } else
                        {
                            place = tiles[13];
                        }
                        background.SetTile(new Vector3Int(x, y, 1), place);
                    }
                    else
                    {
                        background.SetTile(new Vector3Int(x, y, 1), tiles[4]);
                    }
                }
            }
        }

        // Place trees
        for (int x = 2; x < width - 1; x++)
        {
            for (int y = height - 2; y >= 2; y--)
            {
                if (blocks[x, y] && blocks[x + 1, y] && blocks[x, y - 1] && blocks[x + 1, y - 1])
                {
                    obstacles.SetTile(new Vector3Int(x, y - 1, 0), tree);
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
                    obstacles.SetTile(new Vector3Int(x, y, 0), bush);
                    blocks[x, y] = false;
                }
            }
        }
    }
}
