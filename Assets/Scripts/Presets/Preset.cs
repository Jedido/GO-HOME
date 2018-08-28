using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Preset
{
    protected abstract void Init();  // Initialize a Preset. If already initialized, resets the object
    protected abstract bool[,] GetMap();  // Returns the block map
    protected abstract void GenerateObjects(int x, int y, List<GameObject> objects);  // Creates the objects, adding them to the list

    // Sets the map with the bottom left corner being (x, y)
    public void GeneratePreset(int x, int y, bool[,] blocks, List<GameObject> objects)
    {
        Init();
        bool[,] map = GetMap();
        int size = map.GetLength(0) - 1;
        for (int i = 0; i < map.GetLength(1); i++)
        {
            for (int j = 0; j < map.GetLength(0); j++)
            {
                // Map is read this way for easier programming
                blocks[x + i, y + j] = map[size - j, i];
            }
        }
        GenerateObjects(x, y, objects);
    }
}