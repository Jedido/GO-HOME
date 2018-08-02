using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class GenerateMap : MonoBehaviour
{
    public readonly int MIN_SIZE = 16;
    public GameObject UI;

    protected class IntPair
    {
        public int x, y;
        public IntPair(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    protected class Floor
    {
        private Tilemap background, obstacles;
        public Tilemap Background
        {
            get { return background; }
        }
        public Tilemap Obstacles
        {
            get { return obstacles; }
        }

        private GameObject[] objects;
        public GameObject[] Objects
        {
            get { return objects; }
        }

        public Floor(Tilemap bg, Tilemap c, GameObject[] o)
        {
            background = bg;
            obstacles = c;
            objects = o;
        }
    }

    public int Width
    {
        get { return GetWidth(); }
    }
    public int Height
    {
        get { return GetHeight(); }
    }

    private Dictionary<int, Floor> floors;
    private int curFloor;

    // Load the given floor and move the player to the starting point.
    protected abstract int GetWidth();
    protected abstract int GetHeight();
    protected abstract int GetStartingFloor();
    protected abstract Vector3 GetStartingPosition();
    protected abstract Floor LoadNewFloor(int floor);
    protected void AddFloor(int n, Floor floor)
    {
        floors.Add(n, floor);
    }

    protected bool FloorExists(int n)
    {
        return floors.ContainsKey(n);
    }

    protected void Start()
    {
        floors = new Dictionary<int, Floor>();
        PlayerManager.player.currentMap = this;
        PlayerManager.player.StartMap();
        PlayerManager.player.Level = GetStartingFloor();
        // PlayerManager.player.MoveAlien(GetStartingPosition());
        Instantiate(UI);
    }

    public void ChangeFloor(int floor)
    {
        // Try to load the next floor
        // TODO: make the map into a list, if possible
        Floor res;
        if (!floors.TryGetValue(floor, out res))
        {
            res = LoadNewFloor(floor);
            floors.Add(floor, res);
        }

        Floor oldFloor;
        if (floors.TryGetValue(curFloor, out oldFloor))
        {
            oldFloor.Background.gameObject.SetActive(false);
            oldFloor.Obstacles.gameObject.SetActive(false);
            foreach (GameObject o in oldFloor.Objects)
            {
                o.gameObject.SetActive(false);
            }
        }

        res.Background.gameObject.SetActive(true);
        res.Obstacles.gameObject.SetActive(true);
        foreach (GameObject o in res.Objects) {
            o.gameObject.SetActive(true);
        }
        curFloor = floor;
    }

    // Checks if area is greater than specified size
    protected bool IsSizeable(int i, int j, bool[,] blocks)
    {
        bool[,] reachable = new bool[blocks.GetLength(0), blocks.GetLength(1)];
        List<IntPair> list = new List<IntPair>();
        AddOpen(i, j, reachable, blocks, list);
        int size = 0;
        while (list.Count > size && list.Count < MIN_SIZE)
        {
            IntPair next = list[size++];
            int x = next.x;
            int y = next.y;
            AddOpen(x + 1, y, reachable, blocks, list);
            AddOpen(x, y + 1, reachable, blocks, list);
            AddOpen(x - 1, y, reachable, blocks, list);
            AddOpen(x, y - 1, reachable, blocks, list);
        }
        if (list.Count >= MIN_SIZE)
        {
            return true;
        }
        // Area too small
        foreach (IntPair next in list)
        {
            reachable[next.x, next.y] = false;
            blocks[next.x, next.y] = true;
        }
        return false;
    }

    // prereq: IsSizeable(i, j, blocks) == true
    protected void Percolate(int i, int j, bool[,] reachable, bool[,] blocks)
    {
        List<IntPair> list = new List<IntPair>();
        AddOpen(i, j, reachable, blocks, list);
        while (list.Count != 0)
        {
            IntPair next = list[0];
            list.RemoveAt(0);
            int x = next.x;
            int y = next.y;
            AddOpen(x + 1, y, reachable, blocks, list);
            AddOpen(x, y + 1, reachable, blocks, list);
            AddOpen(x - 1, y, reachable, blocks, list);
            AddOpen(x, y - 1, reachable, blocks, list);
        }
    }

    // Adds space if there is no block and it has not been traversed
    private void AddOpen(int x, int y, bool[,] reachable, bool[,] blocks, List<IntPair> list)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height && !blocks[x, y] && !reachable[x, y])
        {
            reachable[x, y] = true;
            list.Add(new IntPair(x, y));
        }
    }
}
