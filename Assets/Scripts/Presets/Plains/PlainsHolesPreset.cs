using System.Collections.Generic;
using UnityEngine;

// A 12x15 Holes obstacle that is in one of the designated areas of the plains.
public class PlainsHolesPreset : Preset {
    private static readonly bool[,] MAP = {
        { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, },
        { false, false, false, false, false, true,  true,  true,  true,  true,  true,  true,  true,  false, false, false, false, false, false, false, },
        { false, false, false, false, true,  true,  false, false, false, false, false, false, true,  true,  false, false, false, false, false, false, },
        { false, false, false, false, false, true,  true,  true,  true,  true,  true,  true,  true,  false, false, false, false, false, false, false, },
        { false, true,  false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true,  false, false, },
        { false, true,  true,  false, false, false, false, false, false, false, false, false, false, false, false, false, true,  true,  false, false, },
        { false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, false, },
        { false, true,  false, false, false, true,  false, false, false, true,  false, false, false, true,  false, false, false, true,  false, false, },
        { false, true,  false, false, false, true,  false, false, false, true,  false, false, false, true,  false, false, false, true,  false, false, },
        { false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, false, },
        { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, },
    };

    private static readonly int HOLE_COUNT = 5;
    private static readonly Vector2[] HOLE_START = {
        new Vector2(3, 5),
        new Vector2(9, 5),
        new Vector2(15, 5),
        new Vector2(6, 6),
        new Vector2(12, 6),
    };
    private static readonly Vector2[] HOLE_END = {
        new Vector2(6, 8),
        new Vector2(3, 2),
        new Vector2(7, 2),
        new Vector2(11, 2),
        new Vector2(15, 2),
    };

    private static readonly int ENEMY_COUNT = 4;
    private static readonly int[] ENEMY_ID = {
        (int)Enemy.EnemyID.RedSlime,
        (int)Enemy.EnemyID.BlueSlime,
        (int)Enemy.EnemyID.YellowSlime,
        (int)Enemy.EnemyID.GreenSlime,
    };
    private static readonly Vector2[] ENEMY_LOCATION = {
        new Vector2(3, 3),
        new Vector2(7, 3),
        new Vector2(11, 3),
        new Vector2(15, 3),
    };

    private static readonly Vector2 DIGSPOT = new Vector2(11, 8);

    private int[] order;
    private GameObject bossUnlock;
    private GameObject portalPair;

    public PlainsHolesPreset(int x, int y, List<GameObject> objects) : base(x, y, objects) { }

    protected override void Init()
    {
        if (portalPair == null)
        {
            portalPair = SpriteLibrary.library.PortalPair;
        }
        if (bossUnlock == null)
        {
            bossUnlock = SpriteLibrary.library.PlainsBossUnlock;
        }

        order = new int[HOLE_COUNT];
        List<int> numbers = new List<int>();
        for (int i = 0; i < HOLE_COUNT; i++)
        {
            numbers.Add(i);
        }
        for (int i = 0; i < HOLE_COUNT; i++)
        {
            int index = Random.Range(0, numbers.Count);
            int next = numbers[index];
            numbers.RemoveAt(index);
            order[i] = next;
        }
    }

    protected override void GenerateObjects()
    {
        for (int i = 0; i < ENEMY_COUNT; i++)
        {
            Vector2 loc = ENEMY_LOCATION[i];
            MakeEnemy(loc.x, loc.y, SpriteLibrary.library.GetEnemy(ENEMY_ID[i]));
        }

        for (int i = 0; i < HOLE_COUNT; i++)
        {
            GameObject pair = Object.Instantiate(portalPair);
            PortalPair pp = pair.GetComponent<PortalPair>();
            pp.StartPosition = HOLE_START[i];
            pp.EndPosition = HOLE_END[order[i]];
            PutObject(0, 0, pair);
        }

        PutObject((int)DIGSPOT.x, (int)DIGSPOT.y, Object.Instantiate(bossUnlock));
    }

    private void MakeEnemy(float x, float y, GameObject prefab)
    {
        GameObject enemy = Object.Instantiate(prefab);
        Enemy e = enemy.GetComponent<Enemy>();
        e.battleSpawn = new GameObject[] { prefab, prefab };
        EnemyDetection detect = enemy.GetComponent<EnemyDetection>();
        detect.MaxRadius = 1;
        detect.DetectionRadius = 1;
        detect.EncounterRadius = 1;
        PutObject(x + 0.5f, y + 0.5f, enemy);
    }

    protected override bool[,] GetMap()
    {
        return MAP;
    }
}
