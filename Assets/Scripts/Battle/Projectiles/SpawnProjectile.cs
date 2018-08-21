using UnityEngine;

public class SpawnProjectile : EnemyProjectile {
    public GameObject[] spawn;

    private void OnDestroy()
    {
        foreach (GameObject o in spawn)
        {
            Instantiate(o, new Vector3(Mathf.Round(transform.position.x - 0.5f), Mathf.Round(transform.position.y - 0.5f)), Quaternion.identity);
        }
    }
}
