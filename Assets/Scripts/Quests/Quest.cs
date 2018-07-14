using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Every action performed is mapped to an integer ID. 
public abstract class Quest {
    private int action, num;
    private Reward reward;

    // Must perform the given "action" for "num" times
    public Quest(int action, int num, Reward reward)
    {
        this.action = action;
        this.num = num;
        this.reward = reward;
    }

    public void Performed(int action)
    {
        if (this.action == action && num != 0)
        {
            num--;
        }
    }

    public bool IsComplete()
    {
        return num == 0;
    }

    public void CompleteQuest()
    {
        if (reward != null)
        {
            reward.GrantReward();
            PlayerManager.player.RemoveQuest(this);
        }
    }
}
