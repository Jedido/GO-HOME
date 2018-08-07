using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour {
    // ALL POSSIBLE REWARDS:
    public enum Type { Gold, Part, KeyItem, SideItem, Item, PlayerStat, Map, BossMap };
    public int type, aux;

    // Shorthand macro
    private PlayerManager p
    {
        get { return PlayerManager.player; }
    }

    public Reward(int type, int aux = 0)
    {
        this.type = type;
        this.aux = aux;
    }

    public void GrantReward()
    {
        switch (type)
        {
            case (int)Type.Gold: p.AddGameStat((int)PlayerManager.GameStats.Gold, aux); PlayerManager.player.Alert("+" + aux + "g", Color.yellow);  break;
            case (int)Type.Part: p.AddGameStat((int)PlayerManager.GameStats.Parts, 1); break;
            case (int)Type.KeyItem: p.EnableKeyItem(aux); break;
            case (int)Type.SideItem: break;  // TODO
            case (int)Type.Item: break;  // TODO
            case (int)Type.PlayerStat: p.UpgradeStat(aux); break;
            case (int)Type.Map: p.EnableMap(aux); break;
            case (int)Type.BossMap: p.SetBossMap(aux, true); PlayerManager.player.Alert("Map \"Dig Site\" found!", new Color(0.4f, 1, 0.6f)); break;
        }
    }
}
