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
    public GameObject alien;

    // Items the player has obtained
    private int[] gameStats;
    public enum GameStats { Day, Gold, Rank, Parts, Keys, Count };

    private bool[] keyItems;
    public enum KeyItems { Sword, Shovel, Pickaxe, Sling, Torch, Pulley,
        Compass, Goggles, Glove, Rope, Fangs, Clothing, Fire, Count };
    private int[][] itemRanks;
    public enum SwordRanks { ATK, Speed, Arc, Count };

    // Maps
    private bool[] maps;
    private bool[] bossEnabled;
    public enum Maps { Plains, Cave, Desert, Mountain, Catacombs, Forest, Depths, Tundra, Volcano, Count };

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

    // Player stats
    private int[] playerStats;
    // regen per 10 seconds
    public enum PlayerStats { TOOLKIT_SIZE, MAX_HP, HP, MS, HP_REGEN, DEFENSE, Count };

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
    public void MoveAlien(Vector3 pos)
    {
        alien.transform.position = position = pos;
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
        keyItems = new bool[(int)KeyItems.Count];

        // TODO: find a better way to do this part?
        itemRanks = new int[3][];
        itemRanks[0] = new int[(int)SwordRanks.Count];

        curQuests = new List<Quest>();
        
        playerStats = new int[(int)PlayerStats.Count];
    }

    public void SaveCharacter(int slot)
    {

    }

    // Load character at slot given
    public void LoadCharacter(int slot)
    {

    }
}
