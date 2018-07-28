using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets up the battle phase
// TODO: add movement outside of battle?
// Default battlefield is 12x9 units, origin in the middle (6, 4.5f)
public abstract class Enemy : MonoBehaviour {
    private Rigidbody2D rb2d;
    // Types of objects in the battlefield
    private GameObject wall, smallProjectile;
    private BattleController battle;

    public abstract int GetID();
    protected abstract void MakeInitial();
    // Be sure to document all IDs here
    public enum EnemyID { Slime, };

    // Since Enemy is never created as a gameobject, this is used instead of Unity's Start()
    protected void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        wall = SpriteLibrary.library.Wall;
        smallProjectile = SpriteLibrary.library.SmallProjectile;
        battle = PlayerManager.player.battle.GetComponent<BattleController>();
    }

    public void InitBattle()
    {
        if (!PlayerManager.player.battle.activeSelf)
        {
            transform.parent = PlayerManager.player.battle.transform;
            transform.position = PlayerManager.player.battle.transform.position + new Vector3(0, 0, 10);
            rb2d.isKinematic = true;
            GetComponent<SpriteRenderer>().enabled = false;
            Instantiate(SpriteLibrary.library.BattlePlayer, transform, false).transform.localPosition = new Vector3(-3, 0);
            GameObject enemy = Instantiate(SpriteLibrary.library.GetEnemy(GetID()), transform, false);
            enemy.transform.localPosition = new Vector3(3, 0);
            enemy.GetComponent<BattleAI>().SetEnemy(this);
            MakeBorder();
            MakeInitial();

            battle.StartBattle();
        }
    }

    protected void MakeBorder()
    {
        // Default border
        battle.SetSize(240, 180);
        int rand = Random.Range(0, 4);

        AddBlock(-118f / 20, 0, 5, 180);
        AddBlock(118f / 20, 0, 5, 180);
        AddBlock(0, 88 / 20f, 240, 5);
        AddBlock(0, -88 / 20f, 240, 5);
    }

    protected void AddBlock(float x, float y, float width, float height)
    {
        GameObject block = Instantiate(wall, transform, true);
        block.transform.localScale = new Vector3(width, height, 1);
        block.transform.localPosition = new Vector3(x, y);
    }

    public void Die()
    {
        battle.EndBattle();
        Destroy(gameObject);
    }
}
