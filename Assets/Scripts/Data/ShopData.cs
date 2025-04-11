using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopItemData
{
    public long itemId;
    public int price;
    public int stock = -1;  // -1 : 무제한
}

[CreateAssetMenu(fileName = "New Shop Data", menuName = "Shop/ShopData")]
public class ShopData : ScriptableObject
{
    public string kName;
    public List<ShopItemData> kListItems;
}
