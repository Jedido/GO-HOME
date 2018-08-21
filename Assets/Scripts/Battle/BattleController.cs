using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {
    private SpriteRenderer battlefield, bg;
    private MeshRenderer text;
    private float flash, flashTimer;
    private Camera battleCam;
    private float fade, delay, delayTimer;
    private bool zoomIn, fadeOut, active;
    private List<Enemy> enemies;
    public bool Active
    {
        get { return active; }
    }

    // Use this for initialization
    void Start() {
        PlayerManager.player.battle = gameObject;
        battlefield = transform.GetChild(1).GetComponent<SpriteRenderer>();
        bg = transform.GetChild(0).GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<MeshRenderer>();
        text.enabled = false;
        battleCam = GetComponent<Camera>();
        active = false;
        gameObject.SetActive(false);
        delay = 0.5f;
        flash = 0.6f;
        enemies = new List<Enemy>();
    }

    private void Update()
    {
        float r = bg.color.r;
        if (r != 0)
        {
            r -= 0.1f;
            if (r < 0)
            {
                r = 0;
            }
            bg.color = new Color(r, 0, 0, 0.4f);
        }

        if (zoomIn && delayTimer < Time.time)
        {
            battleCam.enabled = true;
            if (battleCam.orthographicSize > 6)
            {
                battleCam.orthographicSize = battleCam.orthographicSize * 0.7f;
            }
            if (battleCam.orthographicSize <= 6)
            {
                battleCam.orthographicSize = 6;
                if (flashTimer < Time.time)
                {
                    flashTimer = flash + Time.time;
                    text.enabled = !text.enabled;
                }
                if (Input.anyKeyDown)
                {
                    text.enabled = false;
                    active = true;
                    zoomIn = false;
                    PlayerManager.player.UnpauseTimer();
                }
            }
        }

        if (fadeOut)
        {
            fade -= 0.05f;
            if (fade < 0)
            {
                SetAlpha(0);
                fadeOut = false;
                active = false;
                foreach (Enemy e in enemies)
                {
                    if (e != null)
                    {
                        e.Hide();
                    }
                }
                enemies.Clear();
                gameObject.SetActive(false);
            }
            else
            {
                SetAlpha(fade);
            }
        }

        // TODO: detect if battle is over and stop time + rewards box
    }

    public void SetSize(int width, int height)
    {
        battlefield.transform.localScale = new Vector3(width, height, 1);
    }

    public void HitEffect()
    {
        bg.color = new Color(1, 0, 0, 0.4f);
        // Camera.main.gameObject.GetComponent<CameraController>().shakeDuration = 0.05f;
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

    public void StartBattle(Enemy e, bool transition)
    {
        enemies.Add(e);
        battleCam.orthographicSize = 1000;
        zoomIn = true;
        gameObject.SetActive(true);
        battleCam.enabled = false;
        if (transition)
        {
            // Transition into battle
            delayTimer = delay + Time.time;
        } else
        {
            delayTimer = Time.time;
        }
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
        } else if (collision.tag.Equals("Enemy"))
        {
            Destroy(collision.transform.parent.gameObject);
        } else
        {
            Destroy(collision.gameObject);
        }
    }
}
