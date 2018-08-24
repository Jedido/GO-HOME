using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateSimplePlains : GenerateMap {

    public GameObject layout;  // A preset layout of the plains

    public override int GetID()
    {
        return (int)PlayerManager.Maps.Plains;
    }

    protected override int GetWidth()
    {
        return 0;
    }

    protected override int GetHeight()
    {
        return 0;
    }

    protected override int GetStartingFloor()
    {
        return 0;
    }

    protected override Vector3 GetStartingPosition()
    {
        return new Vector2(42, 38);
    }

    protected override Floor LoadNewFloor(int floor)
    {
        return new Floor(layout.transform.GetChild(0).GetComponent<Tilemap>(), layout.transform.GetChild(1).GetComponent<Tilemap>(), new GameObject[0]);
    }
}
