using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton holding all the objects
public class SpriteLibrary : MonoBehaviour {
    public static SpriteLibrary library;

    public GameObject SpikeTrap
    {
        get { return prefabs["Spike Trap"]; }
    }
    public GameObject Arrow
    {
        get { return prefabs["Arrow"]; }
    }
    public GameObject ArrowTrap
    {
        get { return prefabs["Arrow Trap"]; }
    }
    public GameObject Hole
    {
        get { return prefabs["Hole"]; }
    }
    public GameObject StairsUp
    {
        get { return prefabs["Stairs Up"]; }
    }
    public GameObject StairsDown
    {
        get { return prefabs["Stairs Down"]; }
    }
    public GameObject Switch
    {
        get { return prefabs["Switch"]; }
    }
    public GameObject Shop
    {
        get { return prefabs["Shop"]; }
    }

    // Battle Objects
    public GameObject BattlePlayer
    {
        get { return prefabs["Battle Player"]; }
    }
    public GameObject Wall
    {
        get { return prefabs["Wall"]; }
    }
    public GameObject SmallProjectile
    {
        get { return prefabs["Small Projectile"]; }
    }

    // TODO: take in all the prefabs
    public GameObject[] objects;
    private Dictionary<string, GameObject> prefabs;

    void Awake()
    {
        if (library == null)
            library = this;

        // instance of game already exists
        else if (library != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        Init();
    }

    // Use this for initialization
    void Init() {
        prefabs = new Dictionary<string, GameObject>();
        foreach (GameObject interactable in objects)
        {
            prefabs.Add(interactable.name, interactable);
        }
    }

    public static GameObject Instantiate(GameObject o, int x, int y)
    {
        return Instantiate(o, new Vector3(x, y), Quaternion.identity);
    }
}
