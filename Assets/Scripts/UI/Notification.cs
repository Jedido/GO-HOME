using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour {
    public Text title, content;
    public RectTransform background;

    public string Title
    {
        set { title.text = value; gameObject.SetActive(true); }
    }
    public string Message
    {
        get { return content.text; }
        set {
            content.text = value;
            gameObject.SetActive(true);
            background.localPosition = new Vector2(0, 45);
            background.sizeDelta = new Vector2(300, 100);
            PlayerManager.player.PauseTimer();
        }
    }

    public void Start()
    {
        GameManager.game.notification = this;
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("escape"))
        {
            gameObject.SetActive(false);
            PlayerManager.player.UnpauseTimer();
        }
    }

    public void AddLine(string line)
    {
        content.text += "\n" + line;
        background.position -= new Vector3(0, 25);
        background.sizeDelta += new Vector2(0, 50);
    }

    public void Notify(string name, Color titleColor, string message)
    {
        title.text = name;
        title.color = titleColor;
        Message = message;
    }
}
