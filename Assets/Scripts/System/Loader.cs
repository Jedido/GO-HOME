using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

    public GameObject manager;

    void Awake()
    {
        if (GameManager.game == null) {
            Instantiate(manager);
        }
    }
}
