using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class StageClearModel
{
    public string StageDateId;
    public bool IsCleared;
    public int ClearScore;
    public float ClearTime;
    public int StarCount;
}

[Serializable]
public class PlayerModel
{
    public string PlayerName;
    public int PlayerTotalScore;

    public List<StageClearModel> StageClearList;
}