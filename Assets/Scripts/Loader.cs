using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

    public GameObject gameManager;
    public PlayerManager player;

    void Awake()
    {
        if (GameManager.game == null) {
            Instantiate(gameManager);
        }
    }
}
