using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Every action performed is mapped to an integer ID. 
public class Quest : MonoBehaviour {
    public Text title, description;
    public Image background;
    public Sprite accept;
    private int action, num, n, id;  // id corresponds to index
    private Reward reward;

    public enum Action { SlaySlime, HolesEnd, SlayPlainsBoss };
    // Slay list is in Enemy
    // Retrieve list is in Item (TODO)
    // Reach list is below (TODO)

    private void Awake()
    {
        reward = GetComponent<Reward>();
    }

    // Must perform the given "action" for "num" times
    public void SetQuest(int id, string title, string description, int action, int num)
    {
        this.id = id;
        this.title.text = title;
        this.description.text = description;
        this.action = action;
        this.num = n = num;
    }

    public void SetReward(int type, int aux)
    {
        if (reward == null)
        {
            reward = GetComponent<Reward>();
        }
        reward.type = type;
        reward.aux = aux;
    }

    public void Performed(int action, int times = 1)
    {
        if (this.action == action && num != 0)
        {
            num -= times;
            if (num <= 0)
            {
                PlayerManager.player.Alert("Completed \"" + title.text + "\"", Color.white, 3);
                // TODO: Make a noise?

                // Can accept reward on Request board
                background.sprite = accept;
                background.color = Color.white;
                description.text = "";
            }
        }
    }

    public bool IsComplete()
    {
        return num == 0;
    }

    public void CompleteQuest()
    {
        if (IsComplete() && reward != null)
        {
            reward.GrantReward(true);
            PlayerManager.player.RecordQuestCompletion(id);
            Destroy(gameObject);
        } else if (!IsComplete())
        {
            GameManager.game.notification.Title = title.text;
            GameManager.game.notification.Message = "Progress: " + (n - num) + "/" + n;
        }
    }
}
