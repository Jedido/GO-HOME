using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPair : MonoBehaviour {
	// Use this for initialization
	void Start () {
        Portal child1 = transform.GetChild(0).GetComponent<Portal>();
        Portal child2 = transform.GetChild(1).GetComponent<Portal>();
        Portal.SetPair(child1, child2);
	}
}
