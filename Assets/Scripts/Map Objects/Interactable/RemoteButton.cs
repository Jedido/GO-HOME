using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interacts with a different object once and only once
public class RemoteButton : Remote {
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            Interact();
        }
    }
}
