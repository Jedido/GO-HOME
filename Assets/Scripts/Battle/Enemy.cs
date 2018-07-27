using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy AI during battle
// TODO: add movement outside of battle?
// Default battlefield is 12x9 units, origin in the middle (6, 4.5f)
public abstract class Enemy : MonoBehaviour {
    private Rigidbody2D rb2d;
    private GameObject wall;
    private GameObject smallProjectile;
    private BattleController field;

    public abstract int GetID();
    public enum EnemyID { Slime, };

    // Since Enemy is never created as a gameobject, this is used instead of Unity's Start()
    protected void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        wall = SpriteLibrary.library.Wall;
        smallProjectile = SpriteLibrary.library.SmallProjectile;
        field = PlayerManager.player.battle.GetComponent<BattleController>();
    }

    public void InitBattle()
    {
        if (!PlayerManager.player.battle.activeSelf)
        {
            // field.gameObject.SetActive(true);
            field.StartBattle();
            transform.position = PlayerManager.player.battle.transform.position;
            GetComponent<SpriteRenderer>().enabled = false;
            rb2d.isKinematic = true;
            Instantiate(SpriteLibrary.library.BattlePlayer, transform, false);
            MakeBorder();
            AddThreats();
        }
    }

    protected void MakeBorder()
    {
        // Default border
        field.SetSize(240, 180);
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

    protected abstract void AddThreats();

    protected void Die()
    {
        field.EndBattle();
        Destroy(this);
    }
}
