using UnityEngine;

public class PortalPair : MonoBehaviour {
    private Vector2 start, end;

    public Vector2 StartPosition
    {
        set { start = value; }
    }
    public Vector2 EndPosition
    {
        set { end = value; }
    }

    // Use this for initialization
    void Start () {
        Transform portal1 = transform.GetChild(0);
        Transform portal2 = transform.GetChild(1);
        // Check if they are initialized properly
        if (start != end)
        {
            Transform digSpot = transform.GetChild(2);
            portal2.position = digSpot.position = start;
            portal1.position = end;
        }
        Portal child1 = portal1.GetComponent<Portal>();
        Portal child2 = portal2.GetComponent<Portal>();
        Portal.SetPair(child1, child2);
	}
}
