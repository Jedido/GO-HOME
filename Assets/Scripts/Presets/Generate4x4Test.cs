using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate4x4Test : Preset {
    private static readonly bool[,] MAP = {
            { false, false, false, false },
            { false, true, true, false },
            { false, true, true, false },
            { false, false, false, false }
        };

    protected override void GenerateObjects(int x, int y, List<GameObject> objects)
    {
        GameObject spike = SpriteLibrary.Instantiate(SpriteLibrary.library.SpikeTrap, x, y);
        spike.AddComponent<PressurePlate>();
        GameObject spike1 = SpriteLibrary.Instantiate(spike, x + 1, y);
        GameObject spike2 = SpriteLibrary.Instantiate(spike, x + 2, y);
        GameObject spike3 = SpriteLibrary.Instantiate(spike, x + 3, y);
        GameObject spike4 = SpriteLibrary.Instantiate(spike, x, y + 1);
        GameObject spike5 = SpriteLibrary.Instantiate(spike, x, y + 2);
        GameObject spike6 = SpriteLibrary.Instantiate(spike, x, y + 3);
        GameObject spike7 = SpriteLibrary.Instantiate(spike, x + 1, y + 3);
        GameObject spike8 = SpriteLibrary.Instantiate(spike, x + 2, y + 3);
        GameObject spike9 = SpriteLibrary.Instantiate(spike, x + 3, y + 3);
        GameObject spike10 = SpriteLibrary.Instantiate(spike, x + 3, y + 1);
        GameObject spike11 = SpriteLibrary.Instantiate(spike, x + 3, y + 2);
        objects.Add(spike);
        objects.Add(spike1);
        objects.Add(spike2);
        objects.Add(spike3);
        objects.Add(spike4);
        objects.Add(spike5);
        objects.Add(spike6);
        objects.Add(spike7);
        objects.Add(spike8);
        objects.Add(spike9);
        objects.Add(spike10);
        objects.Add(spike11);
        Debug.Log("Generate4x4Test: Preset added");
    }

    protected override bool[,] GetMap()
    {
        return MAP;
    }
}
