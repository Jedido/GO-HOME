using UnityEngine;

public class Ladder : MonoBehaviour, Interactable {
    private int floor;
    private Vector3 position;

    // Changes the floor based on direction
    public void SetLadder(int floor, Vector3 position)
    {
        this.floor = floor;
        this.position = position;
    }

    public void Interact()
    {
        PlayerManager.player.Level = floor;
        PlayerManager.player.MoveAlien(position);
    }
}
