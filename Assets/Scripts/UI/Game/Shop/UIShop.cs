using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UIShop : MonoBehaviour
{
    public RectTransform kShopContentTransform;
    public GameObject kShopItemUIPrefab;

    private Dictionary<ShopItemData, GameObject> mShopItemDictionary = new Dictionary<ShopItemData, GameObject>();

    public void Init()
    {
        Mng.data.shopItemList.ObserveAdd().Subscribe(_event =>
        {
            var itemObj = Instantiate(kShopItemUIPrefab, kShopContentTransform);
            UIShopItem uiItem = itemObj.GetComponent<UIShopItem>();
            uiItem.SetShopItem(_event.Value);
            mShopItemDictionary[_event.Value] = itemObj;
        }).AddTo(this);
        
        Mng.data.shopItemList.ObserveRemove().Subscribe(_event =>
        {
            // 슬롯 제거
            GameObject obj = mShopItemDictionary[_event.Value];
            mShopItemDictionary.Remove(_event.Value);
            Destroy(obj);

        }).AddTo(this);
        
        Mng.data.shopItemList.ObserveReset().Subscribe(_event =>
        {
            foreach (var obj in mShopItemDictionary.Values)
            {
                Destroy(obj);
            }

            mShopItemDictionary.Clear();
        }).AddTo(this);
    }
}
