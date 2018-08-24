using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// All the generators might be refactored under one superclass 
public class GenerateCave : GenerateMap
{
    public static readonly int STARTING_FLOOR = 3;

    // width and height of map 
    public int n;

    // Value between 0 and 1
    // higher the value, less blocks are spawned
    public float threshold;

    // Higher the value, more compact noise
    public float scaleY;
    public float scaleX;

    public GameObject hole;  // 1x1 portal
    public GameObject ladderUp;  // go up levels
    public GameObject ladderDown;  // go down levels
    public Grid grid;
    public Tilemap tMap, tMapCollide;
    private Dictionary<int, Portal> downLadders, upLadders;  // for connecting portals
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

    public override int GetID()
    {
        return (int)PlayerManager.Maps.Cave;
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
        return STARTING_FLOOR;
    }

    protected override Vector3 GetStartingPosition()
    {
        return new Vector3(n / 2, n / 2);
    }

    protected override Floor LoadNewFloor(int floor)
    {
        // Create the floor
        bool[,] blocks = new bool[n + floor, n + floor];
        bool[,] border = new bool[n + floor, n + floor]; // border tiles look different
        List<GameObject> objects = new List<GameObject>();
        Tilemap background = Instantiate(tMap);
        background.transform.parent = grid.transform;
        Tilemap walls = Instantiate(tMapCollide);
        walls.transform.parent = grid.transform;

        CreateMapLayout(blocks, border, objects, floor);
        SetTileBorder(border, walls);
        GenerateBackground(background, n + floor, n + floor);
        GenerateBlocks(blocks, walls);

        return new Floor(background, walls, objects.ToArray());
    }

    new protected void Start()
    {
        downLadders = new Dictionary<int, Portal>();
        upLadders = new Dictionary<int, Portal>();
        base.Start();
    }

    // Background
    private void GenerateBackground(Tilemap background, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                background.SetTile(new Vector3Int(x, y, 0), Ground);
            }
        }
    }

    private bool IsWall(bool[,] border, int x, int y)
    {
        return x >= border.GetLength(0) || x < 0 || y >= border.GetLength(1) || y < 0 || border[x, y];
    }

    // Borders
    private void SetTileBorder(bool[,] border, Tilemap walls)
    {
        for (int x = 0; x < border.GetLength(0); x++)
        {
            for (int y = 0; y < border.GetLength(1); y++)
            {
                if (border[x, y])
                {
                    bool u = IsWall(border, x, y + 1);
                    bool b = IsWall(border, x, y - 1);
                    bool l = IsWall(border, x - 1, y);
                    bool r = IsWall(border, x + 1, y);
                    bool ul = !IsWall(border, x - 1, y + 1);
                    bool ur = !IsWall(border, x + 1, y + 1);
                    bool bl = !IsWall(border, x - 1, y - 1);
                    bool br = !IsWall(border, x + 1, y - 1);
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
                    else if (bl && br && ur && ul)
                    {
                        continue;
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
                        continue;
                    }
                    walls.SetTile(new Vector3Int(x, y, 1), place);
                }
            }
        }
    }

    // Blocks
    private void GenerateBlocks(bool[,] blocks, Tilemap walls)
    {
        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                if (blocks[x, y])
                {
                    walls.SetTile(new Vector3Int(x, y, 0), bush);
                    blocks[x, y] = false;
                }
            }
        }
    }

    // Takes in 2 empty arrays size nxn each, populates them with map information
    // Returns the player's starting location
    private void CreateMapLayout(bool[,] blocks, bool[,] border, List<GameObject> objects, int floor)
    {
        float offsetX = Random.Range(0, 9999f);
        float offsetY = Random.Range(0, 9999f);
        int n = this.n + floor;

        bool[,] map = ShapeGenerator(n);
        // BorderCalculator(map, border);

        // Debug.Log("Creating map");
        for (int i = 0; i < n; i++)
        {
            border[i, 0] = true;
            border[0, i] = true;
            border[i, n - 1] = true;
            border[n - 1, i] = true;
        }

        // create map via perlin noise
        for (int x = 1; x < n - 1; x++)
        {
            for (int y = 1; y < n - 1; y++)
            {
                // This is where the player starts
                if (x < n / 2 + 5 && x > n / 2 - 5 && y > n / 2 - 5 && y < n / 2 + 5) continue;

                if (!map[x, y])
                {
                    // make walls with blocks
                    // make these things actual walls later
                    border[x, y] = true;
                    continue;
                }

                else
                {
                    float xCoord = x * scaleX + offsetX;
                    float yCoord = y * scaleY + offsetY;

                    float sample = Mathf.PerlinNoise(xCoord, yCoord);

                    if (sample > threshold)
                    {
                        border[x, y] = true;
                        border[x + 1, y] = true;
                        border[x - 1, y] = true;
                        border[x, y + 1] = true;
                        border[x, y - 1] = true;
                    }
                }

            }
        }

        // Debug.Log("Creating descensions");

        // Create a descension hole and a descension exit
        GameObject down = Instantiate(ladderDown, LadderPositionFinder(border), Quaternion.identity);
        objects.Add(down);
        Portal downScript = down.GetComponent<Portal>();
        downScript.SetFloor(-1);
        downLadders.Add(floor, downScript);
        Portal res;
        if (upLadders.TryGetValue(floor - 1, out res))
        {
            Portal.SetPair(downScript, res);
        }

        GameObject up = Instantiate(ladderUp, LadderPositionFinder(border), Quaternion.identity);
        objects.Add(up);
        Portal upScript = up.GetComponent<Portal>();
        upScript.SetFloor(1);
        upLadders.Add(floor, upScript);
        if (downLadders.TryGetValue(floor + 1, out res))
        {
            Portal.SetPair(upScript, res);
        }

        // Debug.Log("Finished descensions");

        // ensure all areas are accessible
        bool[,] reachable = new bool[n, n];
        Percolate(n / 2, n / 2, reachable, border);  // spawn location
        for (int x = 0; x < n; x++)
        {
            for (int y = 0; y < n; y++)
            {
                if (!reachable[x, y] && !border[x, y])
                {
                    // Check if size is large enough
                    bool sizeable = IsSizeable(x, y, border);

                    // create a rabbit hole somewhere (portal)
                    if (sizeable)
                    {
                        int i, j;
                        do
                        {
                            i = (int)(Random.value * (n - 1));
                            j = (int)(Random.value * (n - 1));
                        } while (!reachable[i, j]
                            || (x < n / 2 + 5 && x > n / 2 - 5 && y > n / 2 - 5 && y < n / 2 + 5)
                            || !(i > 2 && j > 2 && i < n - 3 && j < n - 3));
                        GameObject hole1 = Instantiate(hole, new Vector3(i + 0.5f, j + 0.5f), Quaternion.identity);
                        GameObject hole2 = Instantiate(hole, new Vector3(x + 0.5f, y + 0.5f), Quaternion.identity);
                        Portal.SetPair(hole1.GetComponent<Portal>(), hole2.GetComponent<Portal>());
                        objects.Add(hole1);
                        objects.Add(hole2);
                        Percolate(x, y, reachable, border);
                    }
                }
            }
        }
    }

    // calculate the border of the map based on the map
    private void BorderCalculator(bool[,] map, bool[,] border)
    {
        for (int i = 1; i < n - 1; i++)
        {
            bool one = true;
            bool two = true;
            bool three = true;
            bool four = true;
            for (int j = 1; j < n - 1; j++)
            {
                // checking vertically first
                int a1 = n - 1 - j;
                if (one && map[i, j])
                {
                    border[i, j - 1] = true;
                    one = false;
                }
                if (two && map[i, a1])
                {
                    border[i, a1 + 1] = true;
                    two = false;
                }
                // transform into horizontal
                if (three && map[j, i])
                {
                    border[j - 1, i] = true;
                    three = false;
                }
                if (four && map[a1, i])
                {
                    border[a1 + 1, i] = true;
                    four = false;
                }
                if (!(one || two || three || four))
                {
                    break;
                }
            }
        }
    }

    // Return a gameobject of a random location.
    // Return null if extremely unlucky or no spaces possible
    private Vector3 LadderPositionFinder(bool[,] blocks)
    {
        int times = 10000; // to avoid infinite loop for debugging purposes
        int i = 0;
        while (i < times)
        {
            int x = Random.Range(1, blocks.GetLength(0) - 1);
            int y = Random.Range(1, blocks.GetLength(1) - 1);
            if (!blocks[x, y])
            {
                // Debug.Log("found empty place");
                bool sizeable = IsSizeable(x, y, blocks);
                if (sizeable)
                {
                    // Debug.Log("returning");
                    return new Vector3(x + 0.5f, y + 0.5f);
                }
            }
            i++;
        }
        return new Vector3(0, 0);
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

            // Debug.Log("Xrange: " + Xrange + ", Yrange: " + Yrange);

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
