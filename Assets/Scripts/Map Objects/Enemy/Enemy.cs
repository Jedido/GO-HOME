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
    protected bool disableInit, temp;  // setTemp will remove the enemy after battle no matter what
    public GameObject[] battleSpawn; // any additional enemies that are introduced into the battlefield

    // Info
    public GameObject textBox;

    public abstract int GetID();
    public abstract string GetName();
    public abstract void BecomeAggro();
    public abstract void BecomeActive();
    public abstract void BecomeInactive();
    protected abstract void MakeInitial(int number);
    virtual protected Vector3 InitialPosition(int number = 1)
    {
        switch (number)
        {
            case 2: return new Vector2(-3.5f, 0) + Random.insideUnitCircle;
            case 3: return new Vector2(0, 3f) + Random.insideUnitCircle;
            case 4: return new Vector2(0, 3f) + Random.insideUnitCircle;
        }
        return new Vector2(3.5f, 0) + Random.insideUnitCircle;
    }
    protected Vector3 PlayerPosition()
    {
        return new Vector3(0, 0, 10);
    }
    // Be sure to document all IDs here
    public enum EnemyID { RedSlime, BlueSlime, GreenSlime, YellowSlime, WhiteSlime, BlackSlime, Count };

    // Since Enemy is never created as a gameobject, this is used instead of Unity's Start()
    protected void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        wall = SpriteLibrary.library.Wall;
        smallProjectile = SpriteLibrary.library.SProjectile;
        textBox = Instantiate(textBox, transform);
        textBox.transform.localPosition = Vector2.zero;
        Textbox box = textBox.GetComponent<Textbox>();
        box.AddTitle(GetName());
        foreach (GameObject backup in battleSpawn)
        {
            box.AddSub(backup.GetComponent<Enemy>().GetName());
        }
    }

    public void InitBattle(int number = 1, bool disableBlocks = false)
    {
        if (!PlayerManager.player.battle.activeSelf || number != 1)
        {
            if (number == 1)
            {
                Camera.main.GetComponent<CameraController>().CenterOn(gameObject);
                textBox.SetActive(true);
            }
            PlayerManager.player.PauseTimer();
            GetComponent<SpriteRenderer>().sortingOrder = 9;
            battle = PlayerManager.player.battle.GetComponent<BattleController>();
            battleForm = new GameObject();

            battleForm.transform.parent = battle.transform;
            battleForm.transform.position = battle.transform.position + new Vector3(0, 0, 10);
            PlayerManager.player.battleAlien.transform.localPosition = PlayerPosition();

            GameObject enemy = Instantiate(SpriteLibrary.library.GetEnemy(GetID()), battleForm.transform, false);
            enemy.transform.localPosition = InitialPosition(number);
            enemy.GetComponent<BattleCPU>().SetEnemy(this);
            if (!disableBlocks)
            {
                MakeBorder(number);
                MakeInitial(number);
            }

            if (battleSpawn != null)
            {
                foreach (GameObject e in battleSpawn)
                {
                    GameObject aux = Instantiate(e, battle.transform, false);
                    aux.GetComponent<SpriteRenderer>().enabled = false;
                    Enemy auxE = aux.GetComponent<Enemy>();
                    auxE.Start();
                    auxE.temp = true;
                    auxE.InitBattle(++number, disableInit);
                }
            }

            battle.StartBattle(this);
        }
    }

    protected virtual void MakeBorder(int number)
    {
        // TODO: each higher number reduces the number of things spawned
        // Default border
        // battle.SetSize(240, 180);
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
        if (temp)
        {
            Destroy(gameObject);
        } else {
            gameObject.SetActive(false);
        }
    }

    public void Die()
    {
        // TODO: add a respawn timer somewhere and call it
        Destroy(battleForm);
        Destroy(gameObject);
    }

    private void OnMouseOver()
    {
        textBox.SetActive(true);
    }

    private void OnMouseExit()
    {
        textBox.SetActive(false);
    }
}
