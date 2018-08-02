using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour {
    private float maxTime;
    private Image sprite;
    public Sprite[] timeSprites = new Sprite[4];
    private Text text;
    private int phase;

	// Use this for initialization
	void Start () {
        text = transform.GetChild(0).GetComponent<Text>();
        sprite = GetComponent<Image>();
        maxTime = PlayerManager.player.GetGameStat((int)PlayerManager.GameStats.Time);
        phase = 3;
        UpdateTime((int)maxTime);
    }

    // Update is called once per frame
    void Update () {
        int time = (int)PlayerManager.player.GetTime();

        if ((int)(time / maxTime * 4) < phase)
        {
            sprite.sprite = timeSprites[--phase];
        }
        UpdateTime(time);
	}

    private void UpdateTime(int time)
    {
        string min = time / 60 + "";
        int seconds = time % 60;
        string sec = seconds < 10 ? "0" + seconds : seconds + "";
        text.text = min + ":" + sec;
    }
}
