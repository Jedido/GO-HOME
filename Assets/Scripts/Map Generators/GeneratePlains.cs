using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// All the generators might be refactored under one superclass 
public class GeneratePlains : GenerateMap
{
    // TODO: remove all public lowercase fields and add constants
    private static readonly Vector2Int[] CORNER = {
        new Vector2Int(3, 5),
        new Vector2Int(3, 55),
        new Vector2Int(55, 55),
        new Vector2Int(55, 5)
    };

    // width and height of map 
    public int n;

    // Value between 0 and 1
    // higher the value, less blocks are spawned
    public float threshold;

    // Higher the value, more compact noise
    public float scaleY;
    public float scaleX;

    public GameObject hole;  // 1x1 portal
    public Grid grid;
    public Tilemap tMap, tMapCollide;
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

    private GameObject chest;
    private GameObject arrowTrap;
    private GameObject[] enemies;
    private int enemyCount;

    public override int GetID()
    {
        return (int)PlayerManager.Maps.Plains;
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
        return 0;
    }

    protected override Vector3 GetStartingPosition()
    {
        return new Vector3(5, 5);
    }

    // For GeneratePlains, floor == 0 no matter what
    // Returns a new Floor(background, obstacles, player starting position)
    protected override Floor LoadNewFloor(int floor)
    {
        // Create the floor
        bool[,] blocks = new bool[n, n];
        bool[,] presetMap = new bool[n, n];
        int[,] density = new int[n + 1, n + 1]; // keeps track of how many obstacles within 2 spaces
        List<GameObject> objects = new List<GameObject>();
        Tilemap background = Instantiate(tMap);
        background.transform.parent = grid.transform;
        Tilemap obstacles = Instantiate(tMapCollide);
        obstacles.transform.parent = grid.transform;
        CreateMapLayout(blocks, density, objects);
        AddPresets(blocks, presetMap, objects);
        AddObstacles(blocks, presetMap, objects);
        CalculateDensity(blocks, density);
        SetTileWalls(blocks, obstacles);
        SetTileBackground(density, background);
        SetTileBlocks(blocks, obstacles);

        return new Floor(background, obstacles, objects.ToArray());
    }

    new protected void Start()
    {
        base.Start();
        chest = SpriteLibrary.library.SmallChest;
        arrowTrap = SpriteLibrary.library.ArrowTrap;
        enemies = new GameObject[]{
            SpriteLibrary.library.GetEnemy((int)Enemy.EnemyID.RedSlime),
            SpriteLibrary.library.GetEnemy((int)Enemy.EnemyID.BlueSlime),
            SpriteLibrary.library.GetEnemy((int)Enemy.EnemyID.YellowSlime),
            SpriteLibrary.library.GetEnemy((int)Enemy.EnemyID.GreenSlime),
                };
        AddFloor(0, LoadNewFloor(0));
    }

    // Place a tree down (bottom left corner)
    private void TreeBlock(int x, int y, bool[,] blocks, Tilemap obstacles)
    {
        obstacles.SetTile(new Vector3Int(x, y, 0), tree);
        blocks[x, y] = true;
        blocks[x, y + 1] = true;
        blocks[x + 1, y] = true;
        blocks[x + 1, y + 1] = true;
    }

    // Borders
    private void SetTileWalls(bool[,] blocks, Tilemap obstacles)
    {
        for (int i = 2; i < n - 2; i += 2)
        {
            TreeBlock(i, 0, blocks, obstacles);
            TreeBlock(i, n - 2, blocks, obstacles);
        }
        for (int i = 0; i < n; i += 2)
        {
            TreeBlock(0, i, blocks, obstacles);
            TreeBlock(n - 2, i, blocks, obstacles);
        }
    }

    // Takes in 2 empty arrays size nxn each, populates them with map information
    private void CreateMapLayout(bool[,] blocks, int[,] density, List<GameObject> objects)
    {
        float offsetX = Random.Range(0, 9999f);
        float offsetY = Random.Range(0, 9999f);
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

                float xCoord = x * scaleX + offsetX;
                float yCoord = y * scaleY + offsetY;

                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                bool res = sample > threshold;
                blocks[x, y] = res;
            }
        }

        // ensure all areas are accessible
        bool[,] reachable = new bool[n, n];
        Percolate(5, 5, reachable, blocks);  // spawn location
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
                        } while (density[i, j] != 0
                            || !reachable[i, j]
                            || (i < 15 && j < 15)
                            || !(i > 2 && j > 2 && i < n - 3 && j < n - 3));
                        GameObject hole1 = Instantiate(hole, new Vector3(i, j), Quaternion.identity);
                        GameObject hole2 = Instantiate(hole, new Vector3(x, y), Quaternion.identity);
                        Portal.SetPair(hole1.GetComponent<Portal>(), hole2.GetComponent<Portal>());
                        objects.Add(hole1);
                        objects.Add(hole2);
                        Percolate(x, y, reachable, blocks);
                    }
                }
            }
        }
    }

    // Presets
    private void AddPresets(bool[,] blocks, bool[,] covered, List<GameObject> objects)
    {
        // TODO: add presets and put them in here
        int[] order = new int[3];  // putting things into the other corners
        List<int> aux = new List<int>();
        aux.Add(1);
        aux.Add(2);
        aux.Add(3);
        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, aux.Count);
            order[i] = aux[index];
            aux.RemoveAt(index);
        }

        // Corner 1: shop
        new ShopPreset(3, 5, objects).GeneratePreset(blocks, covered);

        // Corner 2: home
        Vector2Int holePos = CORNER[order[0]];
        Preset holes = new PlainsHolesPreset(holePos.x, holePos.y, objects);
        holes.GeneratePreset(blocks, covered);

        // Corner 3: spike path
        Vector2Int pathPos = CORNER[order[1]];
        Preset path = new PlainsSpikePathPreset(pathPos.x, pathPos.y, objects);
        path.Rotation = (order[1] + 1 + Random.Range(0, 2)) % 4;
        path.GeneratePreset(blocks, covered);

        // Corner 4: Miniboss
        Vector2Int miniPos = CORNER[order[2]];
        Preset boss = new PlainsMinibossPreset(miniPos.x, miniPos.y, objects);
        boss.Rotation = (order[2] + 2 + Random.Range(0, 2)) % 4;
        boss.GeneratePreset(blocks, covered);

        // Center
        new HomePreset(25 + Random.Range(0, 5), 25 + Random.Range(0, 5), objects).GeneratePreset(blocks, covered);
    }

    // Enemies and traps
    private void AddObstacles(bool[,] blocks, bool[,] presetMap, List<GameObject> objects)
    {
        // All possible spawn locations
        List<Vector2Int> locs = new List<Vector2Int>();
        for (int i = 2; i < blocks.GetLength(0) - 2; i++)
        {
            for (int j = 2; j < i; j++)
            {
                if (!blocks[i, j] && !presetMap[i, j])
                {
                    locs.Add(new Vector2Int(i, j));
                }
                if (!blocks[j, i] && !presetMap[j, i])
                {
                    locs.Add(new Vector2Int(j, i));
                }
            }
            if (!blocks[i, i] && !presetMap[i, i])
            {
                locs.Add(new Vector2Int(i, i));
            }
        }

        foreach (Vector2Int nextLoc in locs)
        {
            // 1.7% chance to spawn
            if (Random.value < 0.017)
            {
                SpawnObstacle(nextLoc, blocks, objects);
            }
        }
    }
    
    private void SpawnObstacle(Vector2Int nextLoc, bool[,] blocks, List<GameObject> objects)
    {
        int i = nextLoc.x;
        int j = nextLoc.y;
        // Chance for a chest
        if (Random.value < Mathf.Sqrt(enemyCount) / 50f)
        {
            // treasure chest!
            GameObject chest = Instantiate(this.chest, (Vector2)nextLoc, Quaternion.identity);
            Reward reward = chest.GetComponent<Reward>();
            reward.type = (int)Reward.Type.Gold;
            reward.aux = Random.Range(10, 20);
            objects.Add(chest);
        }
        else
        {
            if (!SpawnArrow(nextLoc, blocks, objects))
            {
                // Enemy
                objects.Add(SpawnEnemy(nextLoc));
            }
        }
    }

    private bool SpawnArrow(Vector2Int nextLoc, bool[,] blocks, List<GameObject> objects)
    {
        int i = nextLoc.x;
        int j = nextLoc.y;
        int facing = ArrowLocation(nextLoc, blocks);
        if (facing != -1)
        {
            // Arrow trap
            GameObject trap = Instantiate(arrowTrap, new Vector2(i + 0.5f, j + 0.5f), Quaternion.identity);
            trap.AddComponent<Tripwire>();
            trap.GetComponent<Tripwire>().dir = facing;
            trap.GetComponent<ArrowTrap>().direction = facing;
            objects.Add(trap);
            return true;
        }
        return false;
    }

    // -1 if not possible, number representing direction facing if possible
    private int ArrowLocation(Vector2Int nextLoc, bool[,] blocks, int maxSides = 1)
    {
        int i = nextLoc.x;
        int j = nextLoc.y;

        // Check if arrow trap is possible
        int sides = 0;
        int facing = -1;
        if (blocks[i, j + 1])
        {
            sides++;
            facing = 0;
        }
        if (blocks[i + 1, j])
        {
            sides++;
            facing = 3;
        }
        if (blocks[i, j - 1])
        {
            sides++;
            facing = 2;
        }
        if (blocks[i - 1, j])
        {
            sides++;
            facing = 1;
        }
        if (sides > maxSides)
        {
            return -1;
        }
        return facing;
    }

    private GameObject SpawnEnemy(Vector2 pos)
    {
        GameObject slime = Instantiate(enemies[enemyCount % 4], pos, Quaternion.identity);
        int aux = (int) Mathf.Sqrt(enemyCount) / 2;
        Enemy enemy = slime.GetComponent<Enemy>();
        if (aux > 0)
        {
            enemy.battleSpawn = new GameObject[aux];
            for (int i = 0; i < aux; i++)
            {
                enemy.battleSpawn[i] = enemies[Random.Range(0, 4)];
            }
        }
        enemyCount++;
        return slime;
    }

    // Density
    private void CalculateDensity(bool[,] blocks, int[,] density)
    {
        for (int x = 0; x < n; x++)
        {
            for (int y = 0; y < n; y++)
            {
                if (blocks[x, y])
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
                    // density[x - 2, y]++;
                    // density[x, y - 2]++;
                    // density[x + 2, y]++;
                    // density[x, y + 2]++;
                }
            }
        }
    }

    // Background
    private void SetTileBackground(int[,] density, Tilemap background)
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
    private void SetTileBlocks(bool[,] blocks, Tilemap obstacles)
    {
        // Place trees
        for (int x = 2; x < n - 3; x++)
        {
            for (int y = n - 3; y > 2; y--)
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

        for (int x = 2; x < n - 2; x++)
        {
            for (int y = 2; y < n - 2; y++)
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
