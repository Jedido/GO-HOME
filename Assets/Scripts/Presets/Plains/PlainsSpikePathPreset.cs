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

    private static readonly Vector2 CHEST = new Vector2(15, 4);

    private GameObject spike;

    public PlainsSpikePathPreset(int x, int y, List<GameObject> objects) : base(x, y, objects) { }

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

    protected override void GenerateObjects()
    {
        for (int i = 1; i < WIDTH - 5; i++)
        {
            for (int j = 2; j < HEIGHT - 2; j++)
            {
                if (!map[j, i])
                {
                    MakeSpike(i, HEIGHT - j - 1);
                }
            }
        }

        GameObject chest = Object.Instantiate(SpriteLibrary.library.SmallChest);
        Reward reward = chest.GetComponent<Reward>();
        reward.type = (int)Reward.Type.Gold;
        reward.aux = Random.Range(25, 40);
        PutObject(CHEST.x, CHEST.y, chest);
    }

    private void MakeSpike(int x, int y)
    {
        GameObject s = Object.Instantiate(spike);
        s.AddComponent<PressurePlate>();
        PutObject(x, y, s);
    }

    protected override bool[,] GetMap()
    {
        return map;
    }
}
