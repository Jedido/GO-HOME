using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {
    private SpriteRenderer battlefield;
    private Camera battleCam;
    private float fade, delay, delayTimer;
    private bool zoomIn, fadeOut, active;
    public bool Active
    {
        get { return active; }
    }

    // Use this for initialization
    void Start() {
        PlayerManager.player.battle = gameObject;
        battlefield = transform.GetChild(1).GetComponent<SpriteRenderer>();
        battleCam = GetComponent<Camera>();
        active = false;
        gameObject.SetActive(false);
        delay = 0.5f;
    }

    private void Update()
    {
        if (zoomIn && delayTimer < Time.time)
        {
            if (battleCam.orthographicSize > 6)
            {
                battleCam.orthographicSize = battleCam.orthographicSize * 0.7f;
            }
            if (battleCam.orthographicSize <= 6)
            {
                battleCam.orthographicSize = 6;
                if (Input.anyKeyDown)
                {
                    active = true;
                    zoomIn = false;
                }
            }
        }

        if (fadeOut)
        {
            fade -= 0.03f;
            if (fade < 0)
            {
                SetAlpha(0);
                fadeOut = false;
                active = false;
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
        battleCam.orthographicSize = 1000;
        zoomIn = true;
        delayTimer = delay + Time.time;
        gameObject.SetActive(true);
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
        if (collision.gameObject.tag.Equals("Player"))
        {
            StopEnd();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            EndBattle();
        } else
        {
            Destroy(collision.gameObject);
        }
    }
}
