using UnityEngine;

[System.Serializable]
public class GameDataBase
{
    public string Id;
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

    public float MoveSpeed;
    public float PatrolSpeed;
    public float PatrolRange;

    public float DetectRange;
    public float AttackRange;
    public float AttackCoolTime;
}