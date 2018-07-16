using UnityEngine;

public class Ladder : MonoBehaviour, Interactable {
    private int direction;

    // Changes the floor based on direction
    public void SetDirection(int direction)
    {
        this.direction = direction;
    }

    public void Interact()
    {
        PlayerManager.player.Level += direction;
        Debug.Log(PlayerManager.player.Level);
    }
}
