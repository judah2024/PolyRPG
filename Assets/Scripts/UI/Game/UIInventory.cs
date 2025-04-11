using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnumDef;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public GameObject kSlotUIPrefab;
    public Transform kContent;
    public UISellPopup kUISellPopup;

    List<UIInventorySlot> mUISlotList = new List<UIInventorySlot>();

    void Start()
    {
        for (int i = 0; i < ConstDef.MAX_INVEN_SLOT; i++)
        {
            var obj = Instantiate(kSlotUIPrefab, kContent);
            var uiSlot = obj.GetComponent<UIInventorySlot>();
            uiSlot.Clear();

            Button button = uiSlot.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                //itemInfoUI.SetActive(true);

                if (Mng.play.state == GameState.Shop)
                {
                    kUISellPopup.ShowPopup(uiSlot.slot.item);
                }
                
            });
            mUISlotList.Add(uiSlot);
        }

        Observable.Merge(
                Mng.data.inventory.ObserveReplace().Select(_event => Unit.Default),
                Mng.data.inventory.ObserveCountChanged().Select(_event => Unit.Default))
            .Subscribe(_event => RefreshUI())
            .AddTo(this);
    }

    void RefreshUI()
    {
        var itemList = Mng.data.GetInventory();
        
        foreach (var slot in mUISlotList)
        {
            slot.Clear();
        }

        for (int i = 0; i < itemList.Count; i++)
        {
            mUISlotList[i].UpdateSlot(itemList[i]);
        }
    }

}
