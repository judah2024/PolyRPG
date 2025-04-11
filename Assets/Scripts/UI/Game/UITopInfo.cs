using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.Serialization;

public class UITopInfo : MonoBehaviour
{
    [HideInInspector] public UIGold gold;
    [HideInInspector] public UIPlayerInfo playerInfo;

    private void Awake()
    {
        gold = GetComponentInChildren<UIGold>(true);
        playerInfo = GetComponentInChildren<UIPlayerInfo>(true);
    }

    public void OnInventoryButtonClick()
    {
        Mng.canvas.playerSetting.gameObject.SetActive(true);
    }

    public void OnPauseButtonClick()
    {
        Mng.canvas.pause.gameObject.SetActive(true);
    }
}
