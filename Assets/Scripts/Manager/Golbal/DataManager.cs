using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class DataManager : MonoBehaviour
{
    // TODO : 데이터 매니저는 고결해야한다.
    // -> UI, Model이 뭘 원하든 상관할 바가 아님
    
    // Data     : Equip
    // ㄴ UI    : UIEquip -> EquipSlot
    // ㄴ model : EquipModel
    // ㄴ ability : Caching ability
    // subscription
    static public DataManager Instance;
    
    void Awake()
    {
        gold.Value = 10000;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
        
    }

    public ReactiveProperty<int> gold = new ReactiveProperty<int>();
    public ReactiveProperty<int> exp = new ReactiveProperty<int>();
    

    #region Inventory

    public ReactiveCollection<ItemSlot> inventory = new ReactiveCollection<ItemSlot>();

    public int AddItem(ItemData itemData, int amount)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            var slot = inventory[i];
            if (slot.item.UID != itemData.UID)
                continue;
            
            if (slot.IsCanAddAmount() == false)
                continue;

            int totalAmount = amount + slot.amount;
            int applyAmount = Mathf.Min(totalAmount, ConstDef.MAX_CONSUME_AMOUNT);
            var newSlot = new ItemSlot(itemData, applyAmount);
            inventory[i] = newSlot;
            amount = totalAmount - applyAmount;
        }

        while (amount > 0)
        {
            if (inventory.Count >= ConstDef.MAX_INVEN_SLOT)
                break;
            
            int applyAmount = Mathf.Min(amount, ConstDef.MAX_CONSUME_AMOUNT);
            var newSlot = new ItemSlot(itemData, applyAmount);
            inventory.Add(newSlot);
            amount -= applyAmount;
        }

        return amount;
    }
    public bool RemoveItem(ItemData itemData, int amount)
    {
        if (amount <= 0) 
            return false;
    
        // item 개수 확인
        int sum = inventory
            .Where(_p => _p.item == itemData)
            .Sum(_p => _p.amount);
    
        if (sum < amount)
            return false;
    
        // amount가 작은 슬롯부터 제거
        var slotInfos = inventory
            .Select((slot, index) => new { Slot = slot, Index = index })
            .Where(_p => _p.Slot.item.UID == itemData.UID)
            .OrderBy(_p => _p.Slot.amount)
            .ToList();
    
        int removeAmount = amount;
        List<ItemSlot> removeList = new List<ItemSlot>();
        foreach (var slotInfo in slotInfos)
        {
            if (removeAmount <= 0)
                break;

            int currentAmount = slotInfo.Slot.amount;
            int applyAmount = Mathf.Min(currentAmount, removeAmount);
            int newAmount = currentAmount - applyAmount;
            if (newAmount > 0)
            {
                var newSlot = new ItemSlot(itemData, newAmount);
                inventory[slotInfo.Index] = newSlot;
            }
            else
            {
                removeList.Add(inventory[slotInfo.Index]);
            }

            removeAmount -= applyAmount;
        }

        foreach (var item in removeList)
        {
            inventory.Remove(item);
        }
        
        return true;
    }

    public List<ItemSlot> GetInventory()
    {
        return inventory.ToList();
    }

    #endregion

    #region Shop

    ShopData mCurShopData;
    ShopNPC mCurShopNPC;
    public ReactiveCollection<ShopItemData> shopItemList = new ReactiveCollection<ShopItemData>();

    public void OpenShop(ShopData shopData, ShopNPC npc)
    {
        // UI 표시
        Mng.canvas.itemShop.Open();

        mCurShopData = shopData;
        mCurShopNPC = npc;
        shopItemList.Clear();
        foreach (var item in shopData.kListItems)
        {
            shopItemList.Add(item);
        }
    }

    public void CloseShop()
    {
        Mng.canvas.itemShop.Close();
        shopItemList.Clear();
        mCurShopNPC.StartEndDialogue();
        mCurShopData = null;
        mCurShopNPC = null;
    }
    
    public bool BuyItem(ShopItemData shopItem, int amount)
    {
        int totalPrice = shopItem.price * amount;
        if (gold.Value < totalPrice)
            return false;   // 골드 부족

        if (shopItem.stock != -1 && shopItem.stock < amount)
            return false;   // 재고 부족

        ItemData item =  Mng.table.FindItemData(shopItem.itemId);
        int remainAmount = AddItem(item, amount);
        if (remainAmount > 0)
        {
            int addedAmount = amount - remainAmount;
            if (addedAmount > 0)
            {
                RemoveItem(item, addedAmount);
            }
            return false;   // 구매 실패(배낭이 가득참, 구매 복원)
        }

        gold.Value -= totalPrice;
        if (shopItem.stock != -1)
            shopItem.stock -= amount;

        return true;    // 구매 성공
    }

    public void SellItem(ItemData _itemData, int _amount)
    {
        int totalPrice = _itemData.sellPrice * _amount;
        RemoveItem(_itemData, _amount);
        gold.Value += totalPrice;
    }

    #endregion
}
