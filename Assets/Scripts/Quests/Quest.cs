using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Every action performed is mapped to an integer ID. 
public class Quest {
    private string name, description;
    private int action, num;
    private Reward reward;

    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }

    public enum Action { Slay, Retrieve, Reach };
    // Slay list is in Enemy
    // Retrieve list is in Item (TODO)
    // Reach list is below (TODO)

    // Must perform the given "action" for "num" times
    public Quest(string name, string description, int action, int num, Reward reward)
    {
        this.name = name;
        this.description = description;
        this.action = action;
        this.num = num;
        this.reward = reward;
    }

    public void Performed(int action, int times = 1)
    {
        if (this.action == action && num != 0)
        {
            num -= times;
            if (num <= 0)
            {
                PlayerManager.player.Alert("Completed \"" + name + "\"", Color.white, 3);
                // TODO: Make a noise?
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
            reward = null;
        }
    }
}
