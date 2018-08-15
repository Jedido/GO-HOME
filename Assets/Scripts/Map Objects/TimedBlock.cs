using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedBlock : MonoBehaviour {
    public float time;
    private float timer;

	// Use this for initialization
	void Start () {
        timer = time + Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (timer < Time.time)
        {
            Destroy(gameObject);
        }
	}
}
