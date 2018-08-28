using System.Collections.Generic;
using UnityEngine;

// 19x9 path with spikes
public class PlainsSpikePathPreset : Preset {

    private static readonly int WIDTH = 19;
    private static readonly int HEIGHT = 9;

    private bool[,] map = {
        { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, },
        { false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, },
        { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true,  false, },
        { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true,  false, },
        { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true,  false, },
        { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true,  false, },
        { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true,  false, },
        { false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, },
        { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, },
    };

    private GameObject spike;

    protected override void Init()
    {
        if (spike == null)
        {
            spike = SpriteLibrary.library.SpikeTrap;
        }
        for (int i = 1; i < WIDTH - 5; i += 2)
        {
            int opening = Random.Range(2, 7);
            for (int j = 2; j < HEIGHT - 2; j++)
            {
                map[j, i] = j != opening;
            }
        }
    }

    protected override void GenerateObjects(int x, int y, List<GameObject> objects)
    {
        for (int i = 1; i < WIDTH - 5; i++)
        {
            for (int j = 2; j < HEIGHT - 2; j++)
            {
                if (!map[j, i])
                {
                    objects.Add(MakeSpike(x + i, y + HEIGHT - j - 1));
                }
            }
        }
    }

    private GameObject MakeSpike(int x, int y)
    {
        GameObject s = Object.Instantiate(spike, new Vector2(x, y), Quaternion.identity);
        s.AddComponent<PressurePlate>();
        return s;
    }

    protected override bool[,] GetMap()
    {
        return map;
    }
}
