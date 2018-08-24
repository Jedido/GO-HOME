using UnityEngine;
using System.Collections;

// Specifically used to call GameManager functions
public class ButtonScript : MonoBehaviour {
    public void InitDay()
    {
        GameManager.game.InitDay();
    }

    public void InitLevel(int levelID)
    {
        GameManager.game.InitLevel(levelID);
    }
}
