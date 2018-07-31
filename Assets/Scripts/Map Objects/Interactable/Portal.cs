using UnityEngine;

public class Portal : MonoBehaviour, Interactable {
    private int floor;
    private Portal pair;
    private Vector3 position;
    private Collider2D col;

    private void Start()
    {
        //col = ;
    }

    public static void SetPair(Portal portal1, Portal portal2)
    {
        Vector3 offset = new Vector2(0.5f, 0.5f);
        portal1.position = portal2.gameObject.transform.position + offset;
        portal1.pair = portal2;
        portal2.position = portal1.gameObject.transform.position + offset;
        portal2.pair = portal1;
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
        if (pair != null)
        {
            pair.SetTrigger(true);
        }
    }

    public void Reset()
    {
        
    }

    private void SetTrigger(bool isTrigger)
    {
        GetComponent<Collider2D>().isTrigger = isTrigger;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            SetTrigger(false);
        }
    }
}
