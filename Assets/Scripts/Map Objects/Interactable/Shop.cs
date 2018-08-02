﻿using UnityEngine;

public class Shop : MonoBehaviour, Interactable
{
    public void Interact()
    {
        PlayerManager.player.PauseTimer();
        PlayerManager.player.OpenShop();
    }

    public void Reset()
    {
    }
}
