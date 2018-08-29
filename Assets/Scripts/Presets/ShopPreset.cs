using System.Collections.Generic;
using UnityEngine;

// a 5x5 preset that has the shop in the center.
public class ShopPreset : Preset
{
    private static readonly bool[,] MAP = {
            { false, false, false, false, false },
            { false, false, false, false, false },
            { false, false, false, false, false },
            { false, false, false, false, false },
            { false, false, false, false, false },
        };

    public ShopPreset(int x, int y, List<GameObject> objects) : base(x, y, objects) { }

    protected override void Init()
    {
        Rotation = 0;
    }

    protected override void GenerateObjects()
    {
        PutObject(1, 1, Object.Instantiate(SpriteLibrary.library.Shop));
    }

    protected override bool[,] GetMap()
    {
        return MAP;
    }
}
