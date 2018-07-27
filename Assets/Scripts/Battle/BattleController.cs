using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {
    public static readonly Vector3 BATTLE_POSITION = new Vector3(-500, -500, 0);
    private SpriteRenderer battlefield;
    public CameraController cam;

    // Use this for initialization
    void Start() {
        PlayerManager.player.battle = gameObject;
        battlefield = transform.GetChild(1).GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }

    public void SetSize(int width, int height)
    {
        battlefield.transform.localScale = new Vector3(width, height, 1);
    }

    public void SetColor(Color c)
    {
        battlefield.color = c;
    }

    public void StartBattle()
    {
        gameObject.SetActive(true);
        cam.BattleCam(true);
    }

    public void EndBattle()
    {
        cam.BattleCam(false);
        gameObject.SetActive(false);
    }
}
