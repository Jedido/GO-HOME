using System.Collections.Generic;
using UnityEngine;

public class QuestLoader : MonoBehaviour {
    private class QuestData
    {
        private string title, description;
        private int action, num;
        private int rewardType, aux;

        public QuestData(string title, string description, int action, int num, int rewardType, int aux)
        {
            this.title = title;
            this.description = description;
            this.action = action;
            this.num = num;
            this.rewardType = rewardType;
            this.aux = aux;
        }

        public string Title
        {
            get { return title; }
        }
        public string Description
        {
            get { return description; }
        }
        public int Action
        {
            get { return action; }
        }
        public int Num
        {
            get { return num; }
        }
        public int Type
        {
            get { return rewardType; }
        }
        public int Aux
        {
            get { return aux; }
        }
    }

    public GameObject questTemplate;

    private Dictionary<int, List<QuestData>> quests;

    public void Init() {
        quests = new Dictionary<int, List<QuestData>>();
        List<QuestData> plainsQuests = new List<QuestData>();
        plainsQuests.Add(new QuestData("Cleaning Up", "Slimes have become rampant around the Plains. We are offering a brand new shovel to anyone who slays 5 of them.", (int)Quest.Action.SlaySlime, 5, (int)Reward.Type.KeyItem, (int)PlayerManager.KeyItems.Shovel));
        plainsQuests.Add(new QuestData("Holes", "My lawn was dug up by those pesky moles! Please, find out where they are coming from!", (int)Quest.Action.HolesEnd, 1, (int)Reward.Type.BossMap, (int)PlayerManager.Maps.Plains));
        plainsQuests.Add(new QuestData("A Disturbance", "Recently, there has been an unusual amount of radiation coming from the forest. Can someone check it out?", (int)Quest.Action.SlayPlainsBoss, 1, (int)Reward.Type.Gold, 200));
        plainsQuests.Add(new QuestData("Mystery Statue", "I get the feeling that the statue in the Plains is watching me...I have no request, but the statue may have one.", (int)Quest.Action.SlaySlime, 5, (int)Reward.Type.Gold, 100));  // TODO
        quests[0] = plainsQuests;
    }

    public List<GameObject> GetQuests(int map, bool[] completion)
    {
        List<GameObject> questList = new List<GameObject>();
        for (int i = 0; i < completion.Length; i++)
        {
            if (!completion[i])
            {
                questList.Add(QuickInit(quests[map][i], i));
            }
        }
        return questList;
    }

    private GameObject QuickInit(QuestData data, int id)
    {
        GameObject quest = Instantiate(questTemplate);
        Quest q = quest.GetComponent<Quest>();
        q.SetQuest(id, data.Title, data.Description, data.Action, data.Num);
        q.SetReward(data.Type, data.Aux);
        return quest;
    }
}
