using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAbility : MonoBehaviour
{
    public TMP_Text kHpText;
    public TMP_Text kAtkText;
    public TMP_Text kDefText;
    public TMP_Text kCriText;

    public void ChangeAbility(AbilityData data)
    {
        kHpText.text = $"MaxHp : {data.hp}";
        kAtkText.text = $"Attack : {data.attack}";
        kDefText.text = $"Defence : {data.defence}";
        kCriText.text = $"Critical : {data.critical}";
    }
}
