namespace EnumDef
{
    public enum ItemType
    {
        Equipment,
        Consumable,
        
        Max,
    }
    
    public enum EquipType
    {
        Weapon,
        Shield,
        Head,
        Chest,
        Leg,
        Globe,
        Shoes,
        
        Max,
    }

    public enum ConsumeType
    {
        Potion,
        
        Max
    }
    
    public enum BGMType
    {
        Title,
        Main
    }

    public enum SFXType
    {
        UI_Click,
        UI_Close,
        Attack,
        Hit,
    }

    public enum GameState
    {
        Playing,
        Pause,
        Event,
        Shop,
        
        Max,
    }

    public enum MonsterType
    {
        Skeleton,
        OneEye
    }
    
}