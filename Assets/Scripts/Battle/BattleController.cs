using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {
    private SpriteRenderer battlefield;
    private Camera battleCam;
    private float fade;
    private bool fadeIn, fadeOut;

    // Use this for initialization
    void Start() {
        PlayerManager.player.battle = gameObject;
        battlefield = transform.GetChild(1).GetComponent<SpriteRenderer>();
        battleCam = GetComponent<Camera>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (fadeOut)
        {
            fade -= 0.03f;
            if (fade < 0)
            {
                SetAlpha(0);
                fadeOut = false;
                gameObject.SetActive(false);
            }
            else
            {
                SetAlpha(fade);
            }
        }
    }

    public void SetSize(int width, int height)
    {
        battlefield.transform.localScale = new Vector3(width, height, 1);
    }

    public void SetColor(Color c)
    {
        battlefield.color = c;
    }

    public void SetAlpha(float f)
    {
        Color c = battlefield.color;
        battlefield.color = new Color(c.r, c.g, c.b, f);
    }

    public void StartBattle()
    {
        // Transition into battle
        gameObject.SetActive(true);
        SetAlpha(1);
    }

    public void EndBattle()
    {
        fade = 1;
        fadeOut = true;
    }

    public void StopEnd()
    {
        fadeOut = false;
        SetAlpha(1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Battle Player"))
        {
            StopEnd();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            // just in case something crazy happens
        } else if (collision.gameObject.tag.Equals("Battle Player"))
        {
            EndBattle();
        } else
        {
            Destroy(collision.gameObject);
        }
    }
}
