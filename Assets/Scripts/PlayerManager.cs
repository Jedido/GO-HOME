using UnityEngine;
using System.Collections;

// Keeps track of all metadata on the Player/Game
// In essence, this is the save file.
// This is a singleton along with GameManager.
public class PlayerManager : MonoBehaviour {
    // Singleton (there will only be one instance of PlayerManager)
    private static PlayerManager player = null;

    // Player location on map
    private float x, y, z;

    // Player stats
    private int[] inventory = { 0, 0, 0, 0 };  // item inventory
    // list item constants here (?), or in Item superclass

    private int[] attributes = { 0, 0, 0, 0 };  // player stats
    // list attribute constants

    private int[] stats = {0, 0, 0, 0, 0};  // combat stats
    // list stat constants
    public const int STAT_MAX_HP = 0;
    public const int STAT_MAX_MP = 1;

    // Getters for the stats
    public int gold { get { return 0; } }
    public int maxHP { get { return stats[STAT_MAX_HP]; } }
    private int maxMP { get { return stats[STAT_MAX_MP]; } }
    private float moveSpeed;

    // In Map status
    private float hp;
    private float mp;
    private float hpRegen;
    private float mpRegen;

    // Player buffs (listed out)
    private bool[] buffs;  // Use coroutines for timing
    public const int BUFF_SPEED = 0;

    // Start as new character
    // Default stats here
    public void InitCharacter()
    {
        stats[STAT_MAX_HP] = 100;
        stats[STAT_MAX_MP] = 0;
    }

    // Load character at slot given
    public void LoadCharacter(int slot)
    {

    }

    // Recalculate stats given 
    // Run this after leveling stats
    public void CalculateStats()
    {

    }
}
