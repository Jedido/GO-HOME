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
    public const int SHOP = 3;
    public const int LEVEL = 4;

    // Level Types
    public const int PLAINS = 0;

    public static GameManager game = null;
    private int sceneType;
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

    // Switches scenes
    public void InitGame()
    {
        // Switch to game scene
        SceneManager.LoadScene("Map Select");
    }

    // Loads the shop
    public void LoadShop(int levelID)
    {
        if (sceneType != SHOP) return;
    }

    private string LevelName(int levelID)
    {
        switch (levelID)
        {
            case PLAINS: return "Plains";
            default: return "";
        }
    }

    // Loads a map based on the given levelID
    public void InitLevel()
    {
        if (sceneType != SHOP) return;
        sceneType = LEVEL;
        SceneManager.LoadScene(sceneName);
    }
}