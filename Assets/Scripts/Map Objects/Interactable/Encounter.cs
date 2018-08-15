using UnityEngine;

public class Encounter : MonoBehaviour, Interactable {
    public Enemy enemy;

    public void Interact()
    {
        enemy.InitBattle();
        Destroy(gameObject);
    }

    public void Reset()
    {

    }
}
