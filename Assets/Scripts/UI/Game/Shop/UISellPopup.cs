using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISellPopup : MonoBehaviour
{
    [Header("아이템 정보")]
    public Image kItemIconImage;
    public TMP_Text kItemNameText;
    public TMP_Text kItemPriceText;

    [Header("판매 입력")]
    public TMP_InputField kAmountInput;
    public Button kDecreaseButton;
    public Button kIncreaseButton;

    [Header("판매 결과")]
    public TMP_Text kTotalPriceText;
    public Button kSellButton;

    private ItemData kCurrentItem;
    private int currentAmount = 1;
    private const int MIN_AMOUNT = 1;
    
    public void Init()
    {
        kDecreaseButton.onClick.AddListener(() =>
        {
            currentAmount = Mathf.Max(currentAmount - 1, MIN_AMOUNT);
            UpdateUI();
        });
        
        kIncreaseButton.onClick.AddListener(() =>
        {
            currentAmount++;
            UpdateUI();
        });
        
        kAmountInput.onValueChanged.AddListener(_value =>
        {
            if (int.TryParse(_value, out int amount))
            {
                amount = Mathf.Max(amount, MIN_AMOUNT);
                currentAmount = amount;
                UpdateUI();
            }
        });
        
        kSellButton.onClick.AddListener(() =>
        {
            Mng.data.SellItem(kCurrentItem, currentAmount);
            kCurrentItem = null;
            gameObject.SetActive(false);
        });
    }

    void UpdateUI()
    {
        kAmountInput.text = currentAmount.ToString();
        kTotalPriceText.text = $"{kCurrentItem.sellPrice * currentAmount}";
    }

    public void ShowPopup(ItemData _itemData)
    {
        gameObject.SetActive(true);
        kCurrentItem = _itemData;
        kItemIconImage.sprite = _itemData.icon;
        kItemNameText.text = _itemData.name;
        kItemPriceText.text = $"{_itemData.sellPrice:#,##0} G";
        currentAmount = MIN_AMOUNT;
        UpdateUI();
    }
}
