using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// All the generators might be refactored under one superclass 
public class GenerateCave : GenerateMap
{
    // width and height of map 
    public int n;

    // Value between 0 and 1
    // higher the value, less blocks are spawned
    public float threshold;

    // Higher the value, more compact noise
    public float scaleY;
    public float scaleX;

    public GameObject hole;  // 1x1 portal
    public GameObject ladder;  // go up and down levels
    public Grid grid;
    public Tilemap tMap, tMapCollide;
    private int lastCreatedFloor;
    public Tile[] tiles;
    public Tile bush
    {
        get { return tiles[22]; }
    }
    // 15: Tree
    public Tile tree
    {
        get { return tiles[23]; }
    }
    public Tile Ground
    {
        get { return tiles[Random.Range(13, 22)]; }
    }

    protected override int GetWidth()
    {
        return n;
    }

    protected override int GetHeight()
    {
        return n;
    }

    protected override int GetStartingFloor()
    {
        // Placeholder number, maybe
        return 3;
    }

    protected override Floor LoadNewFloor(int floor)
    {
        // Create the floor
        bool[,] blocks = new bool[n, n];
        bool[,] border = new bool[n, n]; // border tiles look different
        List<GameObject> objects = new List<GameObject>();
        Tilemap background = Instantiate(tMap);
        background.transform.parent = grid.transform;
        Tilemap obstacles = Instantiate(tMapCollide);
        obstacles.transform.parent = grid.transform;

        // 3 things to do before I can finish this up:
        // I deleted the GenerateWalls function, remake it to suit this generator (read next todo)
        // split the border and interior blocks into the two given arrays
        // use the int field lastCreatedLevel to determine where the player should start on the level
        //      e.g. if the player went down, should start at the up ladder.
        CreateMapLayout(blocks, objects);
        GenerateBackground(background);
        GenerateBlocks(blocks, obstacles);

        return new Floor(background, obstacles, objects.ToArray(), new Vector3(n / 2, n / 2));
    }

    // Background
    private void GenerateBackground(Tilemap background)
    {
        for (int x = 0; x < n; x++)
        {
            for (int y = 0; y < n; y++)
            {
                background.SetTile(new Vector3Int(x, y, 0), Ground);
            }
        }
    }

    // Blocks
    private void GenerateBlocks(bool[,] blocks, Tilemap obstacles)
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
    // Returns the player's starting location
    private void CreateMapLayout(bool[,] blocks, List<GameObject> objects)
    {
        float offsetX = Random.Range(0, 9999f);
        float offsetY = Random.Range(0, 9999f);

        // calculate density from borders
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < 3; j++)
            {
            }
        }

        bool[,] map = ShapeGenerator(n);

        Debug.Log("Creating map");

        // create map via perlin noise
        for (int x = 2; x < n - 2; x++)
        {
            for (int y = 2; y < n - 2; y++)
            {
                // This is where the player starts
                if (x < n / 2 + 5 && x > n / 2 - 5 && y > n / 2 - 5 && y < n / 2 + 5) continue;

                if (!map[x, y])
                {
                    // make walls with blocks
                    // make these things actual walls later
                    blocks[x, y] = true;
                    continue;
                }

                else
                {
                    float xCoord = x * scaleX + offsetX;
                    float yCoord = y * scaleY + offsetY;

                    float sample = Mathf.PerlinNoise(xCoord, yCoord);

                    bool res = sample > threshold;
                    blocks[x, y] = res;
                }

            }
        }

        Debug.Log("Creating descensions");
        // Create a descension hole and a descension exit
        GameObject down = DescensionGenerator(blocks);
        down.GetComponent<Ladder>().SetDirection(-1);
        objects.Add(down);
        GameObject up = DescensionGenerator(blocks);
        down.GetComponent<Ladder>().SetDirection(1);
        objects.Add(up);
        // Debug.Log("curlevel " + curLevel);


        Debug.Log("Finished descensions");

        // ensure all areas are accessible
        bool[,] reachable = new bool[n, n];
        Percolate(n / 2, n / 2, reachable, blocks);  // spawn location
        for (int x = 2; x < n - 2; x++)
        {
            for (int y = 2; y < n - 2; y++)
            {
                if (!reachable[x, y] && !blocks[x, y])
                {
                    // Check if size is large enough
                    bool sizeable = IsSizeable(x, y, blocks);

                    // create a rabbit hole somewhere (portal)
                    if (sizeable)
                    {
                        int i, j;
                        do
                        {
                            i = (int)(Random.value * (n - 1));
                            j = (int)(Random.value * (n - 1));
                        } while (!reachable[i, j]
                            || (i < 15 && j < 15)
                            || !(i > 2 && j > 2 && i < n - 3 && j < n - 3));
                        GameObject hole1 = Instantiate(hole, new Vector3(i + 0.5f, j + 0.5f), Quaternion.identity);
                        GameObject hole2 = Instantiate(hole, new Vector3(x + 0.5f, y + 0.5f), Quaternion.identity);
                        Portal.SetPair(hole1.GetComponent<Portal>(), hole2.GetComponent<Portal>());
                        Percolate(x, y, reachable, blocks);
                    }
                }
            }
        }
    }

    // Return a gameobject of a random location.
    // Return null if extremely unlucky or no spaces possible
    private GameObject DescensionGenerator(bool[,] blocks)
    {
        int times = 10000; // to avoid infinite loop for debugging purposes
        int i = 0;
        while (i < times)
        {
            int x = Random.Range(2, n - 2);
            int y = Random.Range(2, n - 2);
            if (!blocks[x, y])
            {
                Debug.Log("found empty place");
                bool sizeable = IsSizeable(x, y, blocks);
                if (sizeable)
                {
                    Debug.Log("returning");
                    return Instantiate(ladder, new Vector3(x + 0.5f, y + 0.5f), Quaternion.identity);
                }
            }
            i++;
        }
        return null;
    }

    // Put n to be at least 30 please
    // Generates a shape I think by taking away rectangles one at a time.
    private bool[,] ShapeGenerator(int n)
    {
        // Initialize variables
        int totalSize = n * n;
        int maxCut = n / 7;
        int minCut = n / 20;
        int mapSizeUpperBound = n * n * 3 / 4;
        int singleUpperBound = n * n / 8;
        bool[,] map = new bool[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                map[i, j] = true;
            }
        }
        List<IntPair> corners = new List<IntPair>();
        corners.Add(new IntPair(0, 0));
        corners.Add(new IntPair(n - 1, 0));
        corners.Add(new IntPair(0, n - 1));
        corners.Add(new IntPair(n - 1, n - 1));

        List<IntPair> cavedCorners = new List<IntPair>();

        while (totalSize > mapSizeUpperBound)
        {
            IntPair c = corners[Random.Range(0, corners.Count)];
            corners.Remove(c);
            // Find its adjacent corners
            IntPair ve = null;
            IntPair ho = null;
            int udist = n;
            int Xdist = n;
            bool Ycave = false;
            bool Xcave = false;
            foreach (IntPair p in corners)
            {
                if (p.x == c.x && udist > Mathf.Abs(p.y - c.y))
                {
                    ve = p;
                    udist = Mathf.Abs(p.y - c.y);
                }
                else if (p.y == c.y && Xdist > Mathf.Abs(p.x - c.x))
                {
                    ho = p;
                    Xdist = Mathf.Abs(p.x - c.x);
                }
            }
            foreach (IntPair p in cavedCorners)
            {
                if (p.x == c.x && udist > Mathf.Abs(p.y - c.y))
                {
                    ve = p;
                    udist = Mathf.Abs(p.y - c.y);
                    Ycave = true;
                }
                else if (p.y == c.y && Xdist > Mathf.Abs(p.x - c.x))
                {
                    ho = p;
                    Xcave = true;
                    Xdist = Mathf.Abs(p.x - c.x);
                }
            }
            /*
            Debug.Log("c coordinate: " + c.x + ", " + c.y);
            Debug.Log("Xdist: " + Xdist + ", Ydist: " + udist);
            */
            // Other corresponding coordinate to make the cut
            // Xdiff are talking about the distance between the two horizontal components
            int Ydiff = Mathf.Abs(ve.y - c.y);
            int Ydir = (ve.y > c.y) ? 1 : -1; // 1 if right, -1 if left
            int Xdiff = Mathf.Abs(ho.x - c.x);
            int Xdir = (ho.x > c.x) ? 1 : -1; // 1 if above, -1 if below

            int Yrange = Mathf.Min(Random.Range(minCut, maxCut), Ydiff);
            // Limit the toal area so nothing too wild happens
            int Xrange = Mathf.Min(Random.Range(minCut, maxCut), singleUpperBound / Yrange, Xdiff);

            Debug.Log("Xrange: " + Xrange + ", Yrange: " + Yrange);

            // This will make it to avoid cutting a peninsula I think
            if ((!Ycave && Yrange == Ydiff) || (!Xcave && Xrange == Xdiff))
            {
                // Add it back before moving on
                corners.Add(c);
                continue;
            }

            int x = c.x + Xrange * Xdir;
            int y = c.y + Yrange * Ydir;
            IntPair cavedRes = new IntPair(x, y);

            // Delete corresponding caved corner if necessary
            // Add corners if necessary as well
            if (Yrange == Ydiff)
            {
                cavedCorners.Remove(ve);
            }
            else
            {
                corners.Add(new IntPair(c.x, y));
            }
            if (Xrange == Xdiff)
            {
                cavedCorners.Remove(ho);
            }
            else
            {
                corners.Add(new IntPair(x, c.y));
            }

            // Remove the rectangle
            RemoveRect(c, cavedRes, map);
            totalSize -= Yrange * Xrange;
            cavedCorners.Add(cavedRes);
            /*
            Debug.Log("new corner is: " + x + ", " + y);
            Debug.Log("corners size is: " + corners.Count);
            Debug.Log("cavedCorner size is: " + cavedCorners.Count);
            Debug.Log("totalCorner size is: " + (cavedCorners.Count + corners.Count));
            */
        }
        return map;
    }

    private void RemoveRect(IntPair a, IntPair b, bool[,] map)
    {
        int lowerX = a.x;
        int higherX = b.x;
        int lowerY = a.y;
        int higherY = b.y;
        if (a.x > b.x)
        {
            lowerX = b.x;
            higherX = a.x;
        }
        if (a.y > b.y)
        {
            lowerY = b.y;
            higherY = a.y;
        }
        for (int i = lowerX; i <= higherX; i++)
        {
            for (int j = lowerY; j <= higherY; j++)
            {
                map[i, j] = false;
            }
        }
    }
}
