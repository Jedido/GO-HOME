using System.Collections.Generic;
using UnityEngine;

public class QuestLoader : MonoBehaviour {
    public GameObject questTemplate;

    public Dictionary<int, List<GameObject>> GetAllQuests()
    {
        Dictionary<int, List<GameObject>> quests = new Dictionary<int, List<GameObject>>();
        List<GameObject> plainsQuests = new List<GameObject>();
        plainsQuests.Add(QuickInit("Cleaning Up", "Slimes have become rampant around the Plains. We are offering a brand new shovel to anyone who slays 5 of them.", (int)Quest.Action.Slay, 5, (int)Reward.Type.KeyItem, (int)PlayerManager.KeyItems.Shovel));
        plainsQuests.Add(QuickInit("Holes", "My lawn was dug up by those pesky moles! Please, find out where they are coming from and I will give you $200.", (int)Quest.Action.Slay, 5, (int)Reward.Type.Gold, 200));
        plainsQuests.Add(QuickInit("A Disturbance", "Recently, there has been an unusual amount of radiation coming from the forest. Can someone check it out?", (int)Quest.Action.Slay, 5, (int)Reward.Type.BossMap, (int)PlayerManager.Maps.Plains));
        plainsQuests.Add(QuickInit("Mystery Statue", "I get the feeling that the statue in the Plains is watching me...I have no request, but the statue may have one.", (int)Quest.Action.Slay, 5, (int)Reward.Type.Gold, 100));
        quests[0] = plainsQuests;
        return quests;
    }

    private GameObject QuickInit(string title, string description, int action, int num, int rewardType, int aux)
    {
        GameObject quest = Instantiate(questTemplate);
        Quest q = quest.GetComponent<Quest>();
        q.SetQuest(title, description, action, num);
        q.SetReward(rewardType, aux);
        return quest;
    }
}
