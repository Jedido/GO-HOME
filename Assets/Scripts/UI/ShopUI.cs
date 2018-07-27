using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour {
    private Transform[] tabs;
    private int curTab;
    private enum Tab { Requests, Shopkeeper, Seer, Blacksmith, Count };

	// Use this for initialization
	void Start () {
        int n = transform.childCount;
        tabs = new Transform[n];
		for (int i = 0; i < n; i++)
        {
            tabs[i] = transform.GetChild(i);
        }
        curTab = -1;
        SwitchTab((int)Tab.Requests);
        PlayerManager.player.curShop = gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        bool close = Input.GetKeyDown("escape");
        if (close)
        {
            gameObject.SetActive(false);
        }
	}

    public void SwitchTab(int tab)
    {
        if (tab != curTab)
        {
            curTab = tab;
            tabs[curTab].transform.SetAsLastSibling();
        }
    }
}
