using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    public Image kIconImage;
    public TMP_Text kAmountText;
    
    [HideInInspector]
    public ItemSlot slot { get; private set; }

    private Button mButton;

    private void Awake()
    {
        mButton = GetComponent<Button>();
    }

    public void UpdateSlot(ItemSlot newSlot)
    {
        slot = newSlot;
        kIconImage.sprite = newSlot.item.icon;
        kIconImage.color = Color.white;
        kAmountText.text = newSlot.amount.ToString();

        mButton.interactable = true;
    }

    public void Clear()
    {
        mButton.interactable = false;
        kIconImage.sprite = null;
        kIconImage.color = Color.clear;
        kAmountText.text = "";
        slot = null;
    }
}
