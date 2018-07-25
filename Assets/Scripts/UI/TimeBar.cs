using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour {
    public Sprite[] timeSprites = new Sprite[4];
    public Text text;   

	// Use this for initialization
	void Start () {
        text.text = "01:00";
    }

    // Update is called once per frame
    void Update () {
	}
}
