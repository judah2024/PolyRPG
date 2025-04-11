using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private static long debugID = 0;
    
    public Player kPlayer;
    
    private void OnTriggerEnter(Collider other)
    {
        int layer = other.gameObject.layer;

        if (layer == LayerMask.NameToLayer("Item"))
        {
            DropItem item = other.GetComponent<DropItem>();
            ItemData data = Mng.table.FindItemData(item.itemId);
            int lastAmount = item.amount;
            item.amount = Mng.data.AddItem(data, item.amount);
            
            int added = lastAmount - item.amount;
            string message = $"{data.name} 을 {added}개 획득하였습니다!";
            Log.Play($"Item Get : {debugID++}");
            Mng.canvas.NotificationList.ShowNotification(message);
            if (item.amount == 0)
            {
                Destroy(other.gameObject);
            }
        }
        else if (layer == LayerMask.NameToLayer("Interactable"))
        {
            ShopNPC npc = other.GetComponent<ShopNPC>();
            kPlayer.FindNPC(npc);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            kPlayer.MissNPC();
        }
    }
}
