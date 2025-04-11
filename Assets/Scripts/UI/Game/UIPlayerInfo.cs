using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour
{
    public TMP_Text kLevelText;
    public Slider kHpBar;
    public Slider kExpBar;

    public void Start()
    {
        Mng.data.exp.Subscribe(_event =>
        {
            int maxExp = 100;
            float r = _event / (float)maxExp;
            kExpBar.value = r;
        });
        
        kLevelText.text = "0";
        kHpBar.value = 1;
        kExpBar.value = 0;
    }

    public void ChangeHp(float hp, float maxHp)
    {
        float r = Mathf.Clamp(hp / maxHp, 0, 1);
        kHpBar.value = r;
    }
    
    public void ChangeExp(float exp, float expLimit)
    {
        float r = Mathf.Clamp(exp / expLimit, 0, 1);
        kHpBar.value = r;
    }
}
