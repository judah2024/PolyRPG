using System;
using EnumDef;
using UniRx;
using UnityEngine;

[Serializable]
public class ItemSlot
{
    public ItemData item;
    
    public int amount ;

    public ItemSlot(ItemData _data = null, int _amount = 0)
    {
        item = _data;
        amount = _amount;
    }

    public void SetItem(ItemData _item)
    {
        item = _item;
        amount = 0;
    }

    public int ChangeAmount(int value)
    {
        int sum = amount + value;
        return sum - amount;
    }

    public bool IsCanAddAmount()
    {
        return item == null || item.isStackable == true;
    }
}