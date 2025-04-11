using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    static public TableManager Instance;

    public ItemTable itemTable;
    public MonsterTable monsterTable;
    public MonsterDropTable MonsterDropTable;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        //Debug.LogError("중복 테이블 매니저 생성");
    }

    public ItemData FindItemData(long _uid)
    {
        var item = itemTable.list.FirstOrDefault(_p => _p.UID == _uid);
        if (item == null)
        {
            Log.Error("찾는 UID 없어요 아이템 테이블에..");
            return null;
        }

        return item;
    }
    
    public ItemData FindItemData(string _name)
    {
        var item = itemTable.list.FirstOrDefault(_p => _p.name == _name);
        if (item == null)
        {
            Log.Error("찾는 UID 없어요 아이템 테이블에..");
            return null;
        }

        return item;
    }

    public MonsterData FindMonsterData(long _uid)
    {
        var monster = monsterTable.list.FirstOrDefault(_p => _p.UID == _uid);
        if (monster == null)
        {
            Log.Error("찾는 UID 없어요 몬스터 테이블에..");
            return null;
        }

        return monster;
    }

    public List<ItemData> FindDropItemList(long _uid)
    {
        var dropList = MonsterDropTable.list.Where(_p => _p.monsterUID == _uid).ToList();
        if (dropList.Count == 0)
        {
            Log.Error("찾는 UID 없어요 몬스터 드롭 테이블에..");
            return null;
        }
        
        List<ItemData> itemList = new List<ItemData>();
        foreach (var data in dropList)
        {
            if (Random.Range(0f, 1f) <= data.rate)
            {
                itemList.Add(FindItemData(data.itemUID));
            }
        }

        return itemList;
    }
}