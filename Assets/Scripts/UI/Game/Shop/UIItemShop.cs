using System;
using System.Collections;
using System.Collections.Generic;
using EnumDef;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIItemShop : MonoBehaviour
{
    public RectTransform kShopUITransform;
    public RectTransform kInventoryUITransform;
    
    float t = 0.25f;

    private UIShop mUIShop;
    private UISellPopup mUISellPopup;
    private CanvasGroup mCanvasGroup;

    public void Init()
    {
        mCanvasGroup = GetComponent<CanvasGroup>();
        mUIShop = GetComponentInChildren<UIShop>(true);
        mUISellPopup = GetComponentInChildren<UISellPopup>(true);
        
        mUIShop.Init();
        mUISellPopup.Init();
        gameObject.SetActive(false);
    }
    
    public void Open()
    {
        gameObject.SetActive(true);
        Mng.sound.PlaySFX(SFXType.UI_Click);
        
        mCanvasGroup.alpha = 0;

        Vector2 fromShop = kShopUITransform.anchoredPosition - Vector2.right * 150;
        Vector2 toShop = kShopUITransform.anchoredPosition;
        kShopUITransform.anchoredPosition =  fromShop;
        LeanTween.move(kShopUITransform, toShop, t);
        
        Vector2 fromInventory = kInventoryUITransform.anchoredPosition + Vector2.right * 150;
        Vector2 toInventory = kInventoryUITransform.anchoredPosition;
        kInventoryUITransform.anchoredPosition =  fromInventory;
        LeanTween.move(kInventoryUITransform, toInventory, t);
        
        LeanTween.alphaCanvas(mCanvasGroup, 1, t);
    }

    public void Close()
    {
        Mng.sound.PlaySFX(SFXType.UI_Close);
        gameObject.SetActive(false);
    }

    public void OnShopExitButtonClick()
    {
        Mng.data.CloseShop();
    }
}
