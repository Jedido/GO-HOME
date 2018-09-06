using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelect : MonoBehaviour {
    public GameObject[] maps;
    public GameObject[] bossMaps;

    // Use this for initialization
    void Start () {
		for (int i = 0; i < maps.Length; i++)
        {
            maps[i].SetActive(PlayerManager.player.MapAccessible(i));
            bossMaps[i].SetActive(PlayerManager.player.BossMapAccessible(i));
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
