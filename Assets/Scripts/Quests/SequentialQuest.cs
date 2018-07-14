using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequentialQuest : Quest {
    private Quest nextQuest;

    public SequentialQuest(int action, int num, Reward reward, Quest next) : base(action, num, reward)
    {
        
    }

    new public void CompleteQuest()
    {
        base.CompleteQuest();
        PlayerManager.player.AddQuest(this);
    }
}
