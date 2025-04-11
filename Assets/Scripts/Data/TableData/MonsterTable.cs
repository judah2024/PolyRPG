using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster Table", menuName = "Table/Monster")]
public class MonsterTable : ScriptableObject
{
    public List<MonsterData> list;
}