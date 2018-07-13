using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    private GameObject exit;
    public GameObject portal
    {
        get { return exit; }
    }

    public void SetPair(GameObject other)
    {
        exit = other;
    }
}
