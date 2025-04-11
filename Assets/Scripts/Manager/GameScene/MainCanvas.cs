using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    static public MainCanvas Instance;

    public UIPause pause { get; private set; }
    public UIPlayerInfo playerInfo { get; private set; }
    public UIItemShop itemShop { get; private set; }
    public UIDialogue dialogue { get; private set; }
    public UIPlayerSetting playerSetting { get; private set; }
    public UINotificationList NotificationList { get; private set; }
    
    void Awake()
    {
        Instance = this;

        pause = GetComponentInChildren<UIPause>(true);
        playerInfo = GetComponentInChildren<UIPlayerInfo>(true);
        itemShop = GetComponentInChildren<UIItemShop>(true);
        dialogue = GetComponentInChildren<UIDialogue>(true);
        playerSetting = GetComponentInChildren<UIPlayerSetting>(true);
        NotificationList = GetComponentInChildren<UINotificationList>(false);
    }

    void Start()
    {
        itemShop.Init();
        playerSetting.Init();
    }

}
