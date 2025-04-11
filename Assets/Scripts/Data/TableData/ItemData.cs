using EnumDef;
using UnityEngine;
using UnityEngine.Serialization;



[System.Serializable]
public class ItemData
{
    public long UID;
    public string name;
    [TextArea(3, 5)]public string description;
    
    // TODO : 오브젝트는 경로로 할것    
    public Sprite icon;
    public string dropPrefabPath;
    public Object equipPrefab;

    public int sellPrice;
    public AbilityData ability;

    public ItemType type;
    public bool isStackable;
}
