using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Image[] hearts = new Image[5];
    public Sprite[] heartSprites = new Sprite[4];
    public Text text;
    public int curHealth;

    // how long to leave the display up after refreshing
    private float displayTime, displayTimer;

    private Image bar;

	// Use this for initialization
	void Start () {
        curHealth = PlayerManager.player.GetPlayerStat((int)PlayerManager.PlayerStats.HP);
        text.text = "" + curHealth;
        displayTime = 2;
        bar = GetComponent<Image>();
        RefreshDisplay();

        // TODO: construct health bar here?
        // Instantiate(new RectTransform());
	}
	
	// Update is called once per frame
	void Update () {
        int health = PlayerManager.player.GetPlayerStat((int)PlayerManager.PlayerStats.HP);
        if (curHealth != health)
        {
            curHealth = health;
            RefreshDisplay();
        }
        if (displayTimer > Time.time)
        {
            SetColor(new Color(1, 1, 1, displayTimer - Time.time));
        } else
        {
            SetColor(new Color(1, 1, 1, 0));
        }
    }

    // Refreshes the health bar display to show the new health amount
    private void RefreshDisplay()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = heartSprites[((curHealth - i + hearts.Length - 1) / hearts.Length)];
        }
        displayTimer = Time.time + displayTime;
        text.text = "" + curHealth;
    }

    private void SetColor(Color c)
    {
        text.color = c;
        bar.color = c;
        foreach (Image heart in hearts)
        {
            heart.color = c;
        }
    }
}
