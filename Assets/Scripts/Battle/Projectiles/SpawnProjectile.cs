using UnityEngine;

public class SpawnProjectile : EnemyProjectile {
    public GameObject[] spawn;

    private void OnDestroy()
    {
        foreach (GameObject o in spawn)
        {
            Instantiate(o, new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)), Quaternion.identity);
        }
    }
}
