using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Textbox : MonoBehaviour {
    private TextMesh sub;
    public GameObject textTemplate;

    // Use this for initialization
    public void Start() {
        transform.position += new Vector3(0.05f, -1f);
        gameObject.SetActive(false);
    }

    public void AddTitle(string s)
    {
        TextMesh text = Instantiate(textTemplate, transform).GetComponent<TextMesh>();
        text.transform.localPosition = Vector3.zero;
        text.text = s;
        text.anchor = TextAnchor.LowerCenter;
    }

    public void AddSub(string s)
    {
        if (sub == null)
        {
            sub = Instantiate(textTemplate, transform).GetComponent<TextMesh>();
            sub.transform.localPosition = Vector3.zero;
            sub.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            sub.text = "";
        } else
        {
            sub.text += "\n";
        }
        sub.text += s;
    }
}
