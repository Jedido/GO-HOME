using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDayUI : MonoBehaviour {
    // Maybe create the Yes/No into a query if there are more questions
    public void Start()
    {
        GameManager.game.endDay = this;
        gameObject.SetActive(false);
    }

    public void Display()
    {
        gameObject.SetActive(true);
    }

    public void Yes()
    {
        PlayerManager.player.EndDay();
        gameObject.SetActive(false);
    }

    public void No()
    {
        gameObject.SetActive(false);
        PlayerManager.player.UnpauseTimer();
    }
}
