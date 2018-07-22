using UnityEngine;

public class PressurePlate : InteractionType {
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            Activate();
        }  
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            Reset();
        }
    }
}
