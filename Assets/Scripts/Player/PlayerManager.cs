using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Keeps track of all metadata on the Player/Game
// In essence, this is the save file.
// This is a singleton along with GameManager.
public class PlayerManager : MonoBehaviour {
    // Singleton (there will only be one instance of PlayerManager)
    public static PlayerManager player = null;
    public GenerateMap currentMap;
    public GameObject curShop, battle, alien, battleAlien;
    private QuestLoader questLoader;

    private bool freeze, inMap, endMap;
    private float time;

    // Items the player has obtained
    private int[] gameStats;
    public enum GameStats { Day, Gold, Time, Rank, Parts, Keys, Count };

    private bool[] keyItems;
    public enum KeyItems { Sword, Shovel, Pickaxe, Sling, Torch, Pulley,
        Compass, Goggles, Glove, Rope, Fangs, Clothing, Fire, Count };
    private int[][] itemRanks;
    public enum SwordRanks { ATK, Speed, Arc, Count };

    // Maps
    private bool[] maps;
    private bool[] bossEnabled;
    public enum Maps { Plains, Cave, Desert, Mountain, City, Forest, Depths, Tundra, Volcano, Catacombs, Count };

    private bool[] eventMaps;
    public enum EventMaps { Hell, Count };

    // Quests
    private Dictionary<int, bool[]> questCompletion;
    public static readonly int[] questCount = { 4 };
    private List<GameObject> curQuests;

    /*
     * Map Phase
     */

    // Player location on map
    private Vector3 position;
    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }
    private int level;
    public int Level
    {
        get { return level; }
        set { if (level != value) { level = value; currentMap.ChangeFloor(level); } }
    }

    // Text
    public GameObject template;
    public static readonly Color textColor = Color.white;


    // Player stats
    private int[] playerStats;
    public enum PlayerStats { TOOLKIT_SIZE, MAX_HP, HP, MS, DEFENSE, Count };

    void Awake()
    {
        if (player == null)
            player = this;

        // instance of game already exists
        else if (player != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        InitCharacter();
    }

    // In Map functions (mainly for interactables)
    public void OpenShop()
    {
        PauseTimer();
        curShop.SetActive(true);
    }

    public void CloseShop()
    {
        UnpauseTimer();
        curShop.SetActive(false);
    }

    public bool CanMove()
    {
        return !freeze
            && (curShop == null || !curShop.activeSelf) 
            && (battle == null || !battle.activeSelf);
    }

    public void MoveAlien(Vector3 pos)
    {
        alien.transform.position = position = pos;
    }

    public float GetTime()
    {
        if (freeze)
        {
            return time;
        }
        return time - Time.time;
    }

    public void PauseTimer()
    {
        if (!freeze)
        {
            freeze = true;
            time -= Time.time;
        }
    }

    public void UnpauseTimer()
    {
        if (freeze)
        {
            freeze = false;
            time += Time.time;
        }
    }

    public void Alert(string message, Color color, float time = 1.5f)
    {
        GameObject obj = Instantiate(template, alien.transform.localPosition + new Vector3(0, 0.7f), Quaternion.identity);
        TextMesh text = obj.GetComponent<TextMesh>();
        text.text = message;
        text.color = color;
        obj.GetComponent<TextFade>().FadeTime = time;
    }

    // Items
    public int GetGameStat(int stat)
    {
        return gameStats[stat];
    }

    public void AddGameStat(int stat, int amount)
    {
        gameStats[stat] += amount;
    }

    public bool HasKeyItem(int item)
    {
        return keyItems[item];
    }

    public void EnableKeyItem(int item)
    {
        keyItems[item] = true;
    }

    public int GetKeyItemRank(int item, int stat)
    {
        return itemRanks[item][stat];
    }

    public void GetItemStat()
    {

    }

    // Maps
    public bool MapAccessible(int map)
    {
        return maps[map];
    }

    public void EnableMap(int map)
    {
        maps[map] = true;
    }

    public void SetBossMap(int map, bool enabled)
    {
        bossEnabled[map] = enabled;
    }

    // Quests
    public void Performed(int action, int num = 1)
    {
        for (int i = 0; i < curQuests.Count; i++)
        {
            GameObject q = curQuests[i];
            if (q == null)
            {
                curQuests.RemoveAt(i);
                i--;
            } else
            {
                q.GetComponent<Quest>().Performed(action, num);
            }
        }
    }

    public List<GameObject> GetQuests()
    {
        // These are displayed on the Requests board
        return curQuests;
    }

    // Stats
    public int GetPlayerStat(int stat) {
        return playerStats[stat];
    }

    public void SetHealth(int amount)
    {
        playerStats[(int)PlayerStats.HP] = amount;
    }

    public void UpgradeStat(int stat)
    {

    }

    // Start as new character
    // Default stats here
    public void InitCharacter()
    {
        gameStats = new int[(int)GameStats.Count];
        gameStats[(int)GameStats.Time] = 180;  // Testing purposes

        keyItems = new bool[(int)KeyItems.Count];
        // keyItems[(int)KeyItems.Shovel] = true;  // Testing purposes

        // TODO: find a better way to do this part?
        itemRanks = new int[3][];
        itemRanks[0] = new int[(int)SwordRanks.Count];

        questLoader = GetComponent<QuestLoader>();
        questLoader.Init();
        // TODO: list all quests here

        playerStats = new int[(int)PlayerStats.Count];
        playerStats[(int)PlayerStats.MAX_HP] = 5;
        playerStats[(int)PlayerStats.HP] = 5;

        maps = new bool[(int)Maps.Count];
        bossEnabled = new bool[(int)Maps.Count];


    }

    public void StartMap()
    {
        inMap = true;
        endMap = false;
        time = Time.time + gameStats[(int)GameStats.Time];
        if (playerStats[(int)PlayerStats.MAX_HP] > playerStats[(int)PlayerStats.HP])
        {
            playerStats[(int)PlayerStats.HP]++;
        }
        gameStats[(int)GameStats.Day]++;

        // Make Quests
        curQuests = questLoader.GetQuests(currentMap.GetID());
    }

    private void Update()
    {
        if (!freeze && inMap && !endMap && time < Time.time)
        {
            EndDay();
        }
        if (inMap && endMap && !freeze)
        {
            inMap = false;
            GameManager.game.InitDay();
        }
    }

    public void EndDay()
    {
        // TODO: display day summary
        // TOOD: penalty for not reaching home in time - no healing, 10% fee
        endMap = true;
        GameManager.game.notification.Notify("NIGHT " + gameStats[(int)GameStats.Day] + " BEGINS", new Color(0.56f, 0.82f, 0.82f), "Click to continue");
   }

    public void SaveCharacter(int slot)
    {

    }

    // Load character at slot given
    public void LoadCharacter(int slot)
    {

    }
}
