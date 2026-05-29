using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MGGameDataManager : MonoBehaviour
{
    public static MGGameDataManager Inst { get; private set; }

    public Dictionary<string, MGMonsterData> MonsterDataDic { get; private set; } = new Dictionary<string, MGMonsterData>();

    [Serializable]
    private class SerializationWrapper<T>
    {
        public List<T> items = new List<T>();
    }

    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }

        Inst = this;

        LoadAll();
    }

    public void LoadAll()
    {
        MonsterDataDic = LoadData<MGMonsterData>("MGMonster");
    }

    private Dictionary<string, T> LoadData<T>(string tableName) where T : GameDataBase
    {
        string resourcePath = $"JsonOutput/{tableName}";

        TextAsset textAsset = Resources.Load<TextAsset>(resourcePath);

        if (textAsset == null)
        {
            Debug.LogError($"[Error] 리소스를 찾을 수 없습니다: Resources/{resourcePath}");
            return new Dictionary<string, T>();
        }

        try
        {
            string jsonString = textAsset.text;
            string wrappedJson = "{\"items\":" + jsonString + "}";

            SerializationWrapper<T> wrapper = JsonUtility.FromJson<SerializationWrapper<T>>(wrappedJson);

            if (wrapper != null && wrapper.items != null)
            {
                Debug.Log($"{typeof(T).Name} 데이터를 {wrapper.items.Count}개 로드했습니다.");

                return wrapper.items.ToDictionary(item => item.Id);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{typeof(T).Name} JSON 로드 오류] {ex.Message}");
        }

        return new Dictionary<string, T>();
    }

    public MGMonsterData GetMonsterData(string dataId)
    {
        if (MonsterDataDic == null || string.IsNullOrEmpty(dataId))
        {
            return null;
        }

        return MonsterDataDic.TryGetValue(dataId, out MGMonsterData data) ? data : null;
    }


}
