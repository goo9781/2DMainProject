using UnityEngine;

[System.Serializable]
public class GameDataBase
{
    public string Id;
}

public enum MGMonsterAttackType
{
    None = 0,
    Melee,
    Ranged
}


[System.Serializable]
public class MGMonsterData : GameDataBase
{
    public string Name;
    public string Description;
    public string IconPath;
    public string PrefabPath;

    public int MaxHp;
    public int AttackDamage;
    public string AttackType;
    public string ProjectilePrefabPath;
    public float ProjectileSpeed;

    public float MoveSpeed;
    public float PatrolSpeed;
    public float PatrolRange;

    public float DetectRange;
    public float AttackRange;
    public float AttackCoolTime;
}