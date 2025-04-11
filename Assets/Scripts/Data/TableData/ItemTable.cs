using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Table", menuName = "Table/Item")]
public class ItemTable : ScriptableObject
{
    public List<ItemData> list;
}

