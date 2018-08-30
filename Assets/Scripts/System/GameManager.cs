using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * A singleton that contains all data on the current game state.
 */
public class GameManager : MonoBehaviour
{
    // Scene Types
    public const int MENU = 0;
    public const int HOME = 1;
    public const int MAPSELECT = 2;
    public const int LEVEL = 3;

    // Level IDs
    public const int PLAINS = 0;
    public const int CAVE = 1;

    public static GameManager game = null;
    public Notification notification;
    private int sceneType;
    private string nextScene;  // name of next scene
    private int day; // current day

    void Awake()
    {
        if (game == null)
            game = this;

        // instance of game already exists
        else if (game != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    /*
     * Game progression is:
     *     1. Menu screen - happens on startup
     *     2. Load + map select - InitDay()
     *     3. shop - InitShop(id)
     *     4. level - InitLevel()
     *     5. back to 2 - ?
     */

    // Switches scenes
    public void InitDay()
    {
        // Switch to game scene
        sceneType = MAPSELECT;
        SceneManager.LoadScene("Map Select");
    }

    // Loads a map based on the shop
    public void InitLevel(int levelID)
    {
        if (sceneType != MAPSELECT) return;
        sceneType = LEVEL;
        SceneManager.LoadScene(LevelName(levelID));
    }

    private string LevelName(int levelID)
    {
        switch (levelID)
        {
            case PLAINS: return "Plains";
            default: return "";
        }
    }
}