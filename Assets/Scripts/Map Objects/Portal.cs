using UnityEngine;

public class Portal : MonoBehaviour, Interactable {
    private Vector3 exitLocation;

    public static void SetPair(Portal portal1, Portal portal2)
    {
        portal1.exitLocation = portal2.gameObject.transform.position;
        portal2.exitLocation = portal1.gameObject.transform.position;
    }

    public void Interact()
    {
        PlayerManager.player.MoveAlien(exitLocation);
    }
}
