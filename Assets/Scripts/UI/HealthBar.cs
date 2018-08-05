using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Image[] hearts = new Image[5];
    public Sprite[] heartSprites = new Sprite[4];
    public Text text;
    public int curHealth;

    private Image bar;

	// Use this for initialization
	void Start () {
        curHealth = PlayerManager.player.GetPlayerStat((int)PlayerManager.PlayerStats.HP);
        text.text = "" + curHealth;
        bar = GetComponent<Image>();
        RefreshDisplay();
	}
	
	// Update is called once per frame
	void Update () {
        int health = PlayerManager.player.GetPlayerStat((int)PlayerManager.PlayerStats.HP);
        if (curHealth != health)
        {
            curHealth = health;
            RefreshDisplay();
        }
    }

    // Refreshes the health bar display to show the new health amount
    private void RefreshDisplay()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = heartSprites[((curHealth - i + hearts.Length - 1) / hearts.Length)];
        }
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
