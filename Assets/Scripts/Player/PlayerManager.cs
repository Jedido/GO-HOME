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

    private bool freeze;
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
    private List<Quest> curQuests;
    // Not sure if these are necessary
    private bool[] sideQuestItems;
    public enum SideQuestItems { Count }
    private bool[] dailyQuestItems;
    public enum DailyQuestItems { Count }

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
        curShop.SetActive(true);
    }

    public bool CanMove()
    {
        return (curShop == null || !curShop.activeSelf) 
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
    public void AddQuest(Quest q)
    {
        curQuests.Add(q);
    }

    public void RemoveQuest(Quest q)
    {
        curQuests.Remove(q);
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
        gameStats[(int)GameStats.Time] = 100;  // Testing purposes

        keyItems = new bool[(int)KeyItems.Count];
        keyItems[(int)KeyItems.Shovel] = true;  // Testing purposes

        // TODO: find a better way to do this part?
        itemRanks = new int[3][];
        itemRanks[0] = new int[(int)SwordRanks.Count];

        curQuests = new List<Quest>();
        
        playerStats = new int[(int)PlayerStats.Count];
        playerStats[(int)PlayerStats.MAX_HP] = 5;
        playerStats[(int)PlayerStats.HP] = 5;

        bossEnabled = new bool[(int)Maps.Count];
    }

    public void StartMap()
    {
        time = gameStats[(int)GameStats.Time];
        if (playerStats[(int)PlayerStats.MAX_HP] > playerStats[(int)PlayerStats.HP])
        {
            playerStats[(int)PlayerStats.HP]++;
        }
    }

    public void SaveCharacter(int slot)
    {

    }

    // Load character at slot given
    public void LoadCharacter(int slot)
    {

    }
}
