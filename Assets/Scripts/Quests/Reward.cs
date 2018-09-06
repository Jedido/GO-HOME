using UnityEngine;

public class Reward : MonoBehaviour {
    // ALL POSSIBLE REWARDS:
    public enum Type { Gold, Part, KeyItem, SideItem, Item, PlayerStat, Map, BossMap, Quest };
    public int type, aux;

    // Shorthand macro
    private PlayerManager p
    {
        get { return PlayerManager.player; }
    }

    public void GrantReward(bool notif = false)
    {
        string res = "";
        Color c = Color.white;
        switch (type)
        {
            case (int)Type.Gold: p.AddGameStat((int)PlayerManager.GameStats.Gold, aux); res = "+" + aux + "g"; c = Color.yellow; break;
            case (int)Type.Part: p.AddGameStat((int)PlayerManager.GameStats.Parts, 1); res = "Obtained spaceship part!"; c = Color.green; break;
            case (int)Type.KeyItem: p.EnableKeyItem(aux); break;  // TODO
            case (int)Type.SideItem: break;  // TODO
            case (int)Type.Item: break;  // TODO
            case (int)Type.PlayerStat: p.UpgradeStat(aux); break;
            case (int)Type.Map: p.EnableMap(aux); break;
            case (int)Type.BossMap: p.SetBossMap(aux, true); res = "Unlocked the Boss Map!"; c = new Color(0.4f, 1, 0.6f); break;
            case (int)Type.Quest: p.Performed(aux);  break;
        }
        if (notif)
        {
            GameManager.game.notification.Title = "Quest Complete!";
            GameManager.game.notification.Message = res;
        }
        else
        {
            PlayerManager.player.Alert(res, c);
        }
    }
}
