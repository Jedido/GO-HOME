using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Every action performed is mapped to an integer ID. 
public class Quest : MonoBehaviour {
    public Text title, description;
    public Image background;
    public Sprite accept;
    private int action, num;
    private Reward reward;

    public enum Action { Slay, Retrieve, Reach };
    // Slay list is in Enemy
    // Retrieve list is in Item (TODO)
    // Reach list is below (TODO)

    private void Awake()
    {
        Reward reward = GetComponent<Reward>();
    }

    // Must perform the given "action" for "num" times
    public void SetQuest(string title, string description, int action, int num)
    {
        this.title.text = title;
        this.description.text = description;
        this.action = action;
        this.num = num;
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
                PlayerManager.player.Alert("Completed \"" + title + "\"", Color.white, 3);
                // TODO: Make a noise?

                // TODO: can accept reward on Request board
                background.sprite = accept;
                background.color = Color.white;
                background.GetComponent<Button>().enabled = true;
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
            reward.GrantReward();
            Destroy(gameObject);
        }
    }
}
