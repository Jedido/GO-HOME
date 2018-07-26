using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Preset {
    // Naming convention:
    // [type][size/Irregular][facing/Open][version]
    // e.g. Push6x6Open2, Path8x3Down0, MathIrregularOpen1
    // note that this is definitely going to change and won't be used as of now
    // feel free to return nothing
    protected abstract bool[,] GetMap();

    protected abstract void GenerateObjects(int x, int y, List<GameObject> objects);

    // Sets the map with the bottom left corner being (x, y)
    public void GeneratePreset(int x, int y, bool[,] blocks, List<GameObject> objects)
    {
        bool[,] map = GetMap();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                blocks[x + i, y + j] = map[i, j];
            }
        }
        GenerateObjects(x, y, objects);
    }
}