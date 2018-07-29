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
    private GameObject battleForm;

    public abstract int GetID();
    public abstract void BecomeActive();
    public abstract void BecomeInactive();
    protected abstract void MakeInitial();
    protected Vector3 InitialPosition()
    {
        return new Vector3(3.5f, 0);
    }
    protected Vector3 PlayerPosition()
    {
        return new Vector3(0, 0, 10);
    }
    // Be sure to document all IDs here
    public enum EnemyID { Slime, };

    // Since Enemy is never created as a gameobject, this is used instead of Unity's Start()
    protected void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        wall = SpriteLibrary.library.Wall;
        smallProjectile = SpriteLibrary.library.SmallProjectile;
    }

    public void InitBattle()
    {
        if (!PlayerManager.player.battle.activeSelf)
        {
            battle = PlayerManager.player.battle.GetComponent<BattleController>();
            battleForm = Instantiate(new GameObject());

            battleForm.transform.parent = PlayerManager.player.battle.transform;
            battleForm.transform.position = PlayerManager.player.battle.transform.position + new Vector3(0, 0, 10);
            PlayerManager.player.battleAlien.transform.localPosition = PlayerPosition();
            GameObject enemy = Instantiate(SpriteLibrary.library.GetEnemy(GetID()), battleForm.transform, false);
            enemy.transform.localPosition = InitialPosition();
            enemy.GetComponent<BattleCPU>().SetEnemy(this);
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
        GameObject block = Instantiate(wall, battleForm.transform, true);
        block.transform.localScale = new Vector3(width, height, 1);
        block.transform.localPosition = new Vector3(x, y);
    }

    public void Hide()
    {
        Destroy(battleForm);
        gameObject.SetActive(false);
    }

    public void Die()
    {
        Destroy(battleForm);
        Destroy(gameObject);
    }
}
