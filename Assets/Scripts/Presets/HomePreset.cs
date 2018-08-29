using System.Collections.Generic;
using UnityEngine;

public class HomePreset : Preset
{
    private static readonly bool[,] MAP = {
            { false, false, false, false },
            { false, false, false, false },
            { false, false, false, false },
            { false, false, false, false },
        };

    public HomePreset(int x, int y, List<GameObject> objects) : base(x, y, objects) { }

    protected override void GenerateObjects()
    {
        PutObject(1, 1, Object.Instantiate(SpriteLibrary.library.Home));
    }

    protected override bool[,] GetMap()
    {
        return MAP;
    }

    protected override void Init()
    {
    }
}
