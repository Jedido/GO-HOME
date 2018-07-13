using UnityEngine;
using System.Collections;

// Specifically used to call GameManager functions
public class ButtonScript : MonoBehaviour {
    public void InitDay()
    {
        GameManager.game.InitDay();
    }

    public void InitShop(int levelID)
    {
        GameManager.game.InitShop(levelID);
    }

    public void InitLevel()
    {
        GameManager.game.InitLevel();
    }
}
