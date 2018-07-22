using UnityEngine;

public class Portal : MonoBehaviour, Interactable {
    private int floor;
    private Vector3 position;

    public static void SetPair(Portal portal1, Portal portal2)
    {
        portal1.position = portal2.gameObject.transform.position;
        portal2.position = portal1.gameObject.transform.position;
    }

    // Changes the floor based on direction
    public void SetFloor(int floor)
    {
        this.floor = floor;
    }

    public void Interact()
    {
        PlayerManager.player.Level += floor;
        PlayerManager.player.MoveAlien(position);
    }

    public void Reset()
    {
        
    }
}
