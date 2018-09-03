using UnityEngine;

public class Home : MonoBehaviour, Interactable
{
    public void Interact()
    {
        GameManager.game.endDay.Display();
        PlayerManager.player.PauseTimer();
    }

    public void Reset()
    {
        PlayerManager.player.UnpauseTimer();
    }
}
