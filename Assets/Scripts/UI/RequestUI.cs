using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Displays up to 4 quests on the Request Board.
public class RequestUI : MonoBehaviour {
    private static readonly int START_X = 640;
    private static readonly int START_Y = 630;
    private static readonly int HEIGHT = 130;
    private List<GameObject> quests;

    public void Start()
    {
        UpdateQuests();
    }

    public void Update()
    {
    }

    private void UpdateQuests()
    {
        quests = PlayerManager.player.GetQuests();
        int size = quests.Count;
        for (int i = 0; i < 4 && i < size; i++)
        {
            GameObject quest = quests[i];
            quest.GetComponent<RectTransform>().localPosition = new Vector2(START_X, START_Y - HEIGHT * i);
            quest.transform.SetParent(transform);
        }
    }
}
