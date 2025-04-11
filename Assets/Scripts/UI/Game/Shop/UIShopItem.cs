using System.Collections;
using System.Collections.Generic;
using EnumDef;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour
{
    public Image kIcon;
    public TMP_Text kPriceText;
    public TMP_Text kNameText;
    public TMP_Text kDescText;

    private ShopItemData mShopItemData;

    public void SetShopItem(ShopItemData shopItemData)
    {
        mShopItemData = shopItemData;

        ItemData item = Mng.table.FindItemData(shopItemData.itemId);
        kIcon.sprite = item.icon;
        kPriceText.text = mShopItemData.price.ToString("#,##0 G");
        kNameText.text = item.name;
        kDescText.text = item.description;
    }

    public void OnBuyButtonClick()
    {
        Mng.sound.PlaySFX(SFXType.UI_Click);
        bool result = Mng.data.BuyItem(mShopItemData, 1);
        Debug.Log(result ? "구매 성공!" : "구매 실패...");
    }
}
