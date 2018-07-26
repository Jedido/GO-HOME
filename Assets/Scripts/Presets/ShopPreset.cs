using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopPreset : Preset
{
    private static readonly bool[,] MAP = {
            { false, false, false, false, false },
            { false, false, false, false, false },
            { false, false, false, false, false },
            { false, false, false, false, false },
            { false, false, false, false, false },
        };

    protected override void GenerateObjects(int x, int y, List<GameObject> objects)
    {
        objects.Add(SpriteLibrary.Instantiate(SpriteLibrary.library.Shop, x + 1, y + 1));
    }

    protected override bool[,] GetMap()
    {
        return MAP;
    }
}
