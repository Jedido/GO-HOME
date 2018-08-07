using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFade : MonoBehaviour {
    private TextMesh text;
    private float time;
    public float FadeTime
    {
        set { time = value; }
    }
    private Color c;

	// Use this for initialization
	void Start () {
        text = GetComponent<TextMesh>();
        c = text.color;
	}
	
	// Update is called once per frame
	void Update () {
        if (time < Time.time)
        {
            Destroy(gameObject);
        }
        else if (time - Time.time < 1)
        {
            transform.position += new Vector3(0, 0.005f);
        }
        text.color = new Color(c.r, c.g, c.b, time - Time.time);
	}

}
