[System.Serializable]
public class MonsterData
{
    public int UID;
    
    public string name;
    public string prefabPath;
    public int money;
    public int exp;

    public EnumDef.MonsterType type; 
    public AbilityData ability;
    public float attackRange;
}
