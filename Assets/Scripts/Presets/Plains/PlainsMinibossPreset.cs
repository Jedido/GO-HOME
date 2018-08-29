using System.Collections.Generic;
using UnityEngine;

public class PlainsMinibossPreset : Preset {
    private static readonly bool[,] MAP = {
        { false, false, false, false, false, false, false, false, false, false, false, },
        { false, false, false, true,  true,  true,  true,  true,  false, false, false, },
        { false, true,  true,  true,  false, false, false, true,  true,  true,  false, },
        { false, true,  false, false, false, false, false, false, false, true,  false, },
        { false, true,  true,  false, false, false, false, false, true,  true,  false, },
        { false, true,  true,  true,  true,  false, true,  true,  true,  true,  false, },
        { false, false, false, false, false, false, false, false, false, false, false, },
    };

    private static readonly Vector2 BOSS = new Vector2(5, 1);

    private static readonly Vector2 CHEST = new Vector2(5, 3);

    private GameObject enemy;

    public PlainsMinibossPreset(int x, int y, List<GameObject> objects) : base(x, y, objects) { } 

    protected override void GenerateObjects()
    {
        PutObject(BOSS.x + 0.5f, BOSS.y + 0.5f, Object.Instantiate(enemy));
        GameObject chest = Object.Instantiate(SpriteLibrary.library.SmallChest);
        Reward reward = chest.GetComponent<Reward>();
        reward.type = (int)Reward.Type.Gold;
        reward.aux = Random.Range(70, 100);
        PutObject(CHEST.x, CHEST.y, chest);
    }

    protected override bool[,] GetMap()
    {
        return MAP;
    }

    protected override void Init()
    {
        enemy = SpriteLibrary.library.GetEnemy((int)Enemy.EnemyID.WhiteSlime + Random.Range(0, 2));
    }
}
