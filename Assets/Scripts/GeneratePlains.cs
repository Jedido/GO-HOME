using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// All the generators might be refactored under one superclass 
public class GeneratePlains : MonoBehaviour
{
    // width and height of map 
    public int n;

    // Value between 0 and 1
    // higher the value, less blocks are spawned
    public float threshold;

    // Higher the value, more compact noise
    public float scaleY;
    public float scaleX;

    // Randomized in order to sample the perlin noise from a different area each time
    private float offsetX;
    private float offsetY;

    public GameObject hole;  // 1x1 portal
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

    private class IntPair {
        public int x, y;
        public IntPair(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    void Start()
    {
        offsetX = Random.Range(0, 9999f);
        offsetY = Random.Range(0, 9999f);
        GenerateTerrain();
    }

    // Borders
    void GenerateWalls()
    {
        for (int i = 2; i < n - 2; i += 2)
        {
            obstacles.SetTile(new Vector3Int(i, 0, 0), tree);
            obstacles.SetTile(new Vector3Int(i, n - 2, 0), tree);
        }
        for (int i = 0; i < n; i += 2)
        {
            obstacles.SetTile(new Vector3Int(0, i, 0), tree);
            obstacles.SetTile(new Vector3Int(n - 2, i, 0), tree);
        }
    }

    // Background
    private void GenerateBackground(int[,] density)
    {
        for (int x = 0; x < n; x++)
        {
            for (int y = 0; y < n; y++)
            {
                if (density[x, y] == 0)
                {
                    background.SetTile(new Vector3Int(x, y, 1), tiles[13]);
                }
                else
                {
                    if (x > 2 && y > 2 && x < n - 2 && y < n - 2)
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
                            }
                            else if (bl && br)
                            {
                                place = tiles[7];
                            }
                            else if (bl && ul)
                            {
                                place = tiles[3];
                            }
                            else if (br && ur)
                            {
                                place = tiles[5];
                            }
                            else if (ul)
                            {
                                place = tiles[12];
                            }
                            else if (ur)
                            {
                                place = tiles[11];
                            }
                            else if (bl)
                            {
                                place = tiles[10];
                            }
                            else if (br)
                            {
                                place = tiles[9];
                            }
                            else
                            {
                                place = tiles[4];
                            }
                        }
                        else if (b && l && r && !bl && !br)
                        {
                            place = tiles[1];
                        }
                        else if (u && l && r && !ul && !ur)
                        {
                            place = tiles[7];
                        }
                        else if (u && b && l && !bl && !ul)
                        {
                            place = tiles[5];
                        }
                        else if (u && b && r && !br && !ur)
                        {
                            place = tiles[3];
                        }
                        else if (u && r && !ur)
                        {
                            place = tiles[6];
                        }
                        else if (u && l && !ul)
                        {
                            place = tiles[8];
                        }
                        else if (b && r && !br)
                        {
                            place = tiles[0];
                        }
                        else if (b && l && !bl)
                        {
                            place = tiles[2];
                        }
                        else
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
    }

    // Blocks
    private void GenerateBlocks(bool[,] blocks)
    {
        // Place trees
        for (int x = 2; x < n - 1; x++)
        {
            for (int y = n - 2; y >= 2; y--)
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

        for (int x = 2; x < n - 1; x++)
        {
            for (int y = 2; y < n - 1; y++)
            {
                if (blocks[x, y])
                {
                    obstacles.SetTile(new Vector3Int(x, y, 0), bush);
                    blocks[x, y] = false;
                }
            }
        }
    }

    // Takes in 2 empty arrays size nxn each, populates them with map information
    private void CreateMap(bool[,] blocks, int[,] density)
    {
        // calculate density from borders
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                density[i, j] = 5;
                density[i, n - 1 - j] = 5;
                density[j, i] = 5;
                density[n - 1 - j, i] = 5;
            }
            density[i, 3]++;
            density[3, i]++;
            density[i, n - 4]++;
            density[n - 4, i]++;
        }

        // create map via perlin noise
        for (int x = 2; x < n - 2; x++)
        {
            for (int y = 2; y < n - 2; y++)
            {
                // This is where the player starts
                if (x < 15 && y < 15) continue;

                float xCoord = (float)x / 100 * scaleX + offsetX;
                float yCoord = (float)y / 100 * scaleY + offsetY;

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
                            if (i > 0 && i < n && j > 0 && j < n)
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

        // ensure all areas are accessible
        bool[,] reachable = new bool[n, n];
        Percolate(5, 5, reachable, blocks);  // spawn location
        for (int x = 2; x < n - 2; x++)
        {
            for (int y = 2; y < n - 2; y++)
            {
                if (!reachable[x, y] && !blocks[x,y])
                {
                    // create a rabbit hole somewhere (portal)
                    int i, j;
                    do
                    {
                        i = (int)(Random.value * (n - 1));
                        j = (int)(Random.value * (n - 1));
                    } while (!reachable[i, j] || !(i < 15 && j < 15) || !(i > 2 && j > 2 && i < n - 3 && j < n - 3));
                    GameObject hole1 = Instantiate(hole, new Vector3(i + 0.5f, j + 0.5f), Quaternion.identity);
                    GameObject hole2 = Instantiate(hole, new Vector3(x + 0.5f, y + 0.5f), Quaternion.identity);
                    hole1.GetComponent<Portal>().SetPair(hole2);
                    hole2.GetComponent<Portal>().SetPair(hole1);
                    Percolate(x, y, reachable, blocks);
                }
            }
        }
    }

    private void Percolate(int i, int j, bool[,] reachable, bool[,] blocks)
    {
        List<IntPair> list = new List<IntPair>();
        AddOpen(i, j, reachable, blocks, list);
        int step = 0;
        while (list.Count != 0)
        {
            step++;
            IntPair next = list[0];
            int x = next.x;
            int y = next.y;
            AddOpen(x + 1, y, reachable, blocks, list);
            AddOpen(x, y + 1, reachable, blocks, list);
            AddOpen(x - 1, y, reachable, blocks, list);
            AddOpen(x, y - 1, reachable, blocks, list);
            list.RemoveAt(0);
        }
    }

    // Adds space if there is no block and it has not been traversed
    private void AddOpen(int x, int y, bool[,] reachable, bool[,] blocks, List<IntPair> list)
    {
        if (x > 1 && x < n - 2 && y > 1 && y < n - 2 && !blocks[x, y] && !reachable[x, y])
        {
            reachable[x, y] = true;
            list.Add(new IntPair(x, y));
        }
    }

    // TODO add exit (one of the other 3 corners) and make sure it is reachable (percolate)
    void GenerateTerrain()
    {
        bool[,] blocks = new bool[n, n];
        int[,] density = new int[n + 1, n + 1]; // keeps track of how many obstacles within 2 spaces

        CreateMap(blocks, density);  // tilemap
        GenerateWalls();
        GenerateBackground(density);
        GenerateBlocks(blocks);
    }

}
