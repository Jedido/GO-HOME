using UnityEngine;
using UnityEngine.UI;

public class DayText : MonoBehaviour {
    private Image image;
    private Text text;
    private static readonly float fadeDelay = 0.7f;
    private float fadeTimer;
    private bool fade;

    private void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        text.text = "Day " + PlayerManager.player.GetGameStat((int)PlayerManager.GameStats.Day);
        PlayerManager.player.PauseTimer();
        fadeTimer = fadeDelay + Time.time;
        Fade();
    }

    private void Update()
    {
        if (!fade && Input.anyKeyDown)
        {
            fadeTimer = fadeDelay + Time.time;
            fade = true;
            PlayerManager.player.UnpauseTimer();
        }
        if (fade)
        {
            Fade();
        }
    }

    private void Fade()
    {
        float a = fadeTimer - Time.time;
        if (a < 0)
        {
            Destroy(gameObject);
        }
        else
        {
            if (a > 0.7f) a = 0.7f;
            image.color = new Color(0, 0, 0, a);
            text.color = new Color(1, 1, 1, a);
        }
    }
}
