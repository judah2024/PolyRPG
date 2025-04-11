using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster Drop Table", menuName = "Table/MonsterDrop")]
public class MonsterDropTable : ScriptableObject
{
    public List<MonsterDropData> list;
}
