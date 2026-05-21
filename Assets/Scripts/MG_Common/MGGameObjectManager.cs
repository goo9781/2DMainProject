using System.Collections.Generic;
using UnityEngine;

public class MGGameObjectManager : MonoBehaviour
{
    public static MGGameObjectManager Inst { get; private set; }

    [Header("몬스터 생성 설정")]
    [SerializeField] private GameObject Prefab_Monster;
    [SerializeField] private Transform Root_Monster;

    private int _objectInstanceKeyGenerator = 0;

    private Dictionary<int, GameObject> _createdGameObjectDic = new Dictionary<int, GameObject>();

    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }

        Inst = this;
    }
    private int GenerateInstanceId()
    {
        _objectInstanceKeyGenerator++;
        return _objectInstanceKeyGenerator;
    }

    public GameObject RequestSpawnMonster(Vector3 spawnPosition)
    {
        if (Prefab_Monster == null)
        {
            Debug.LogWarning("몬스터 프리팹이 등록되지 않았습니다.");
            return null;
        }

        Transform parent = Root_Monster != null ? Root_Monster : transform;

        GameObject monsterObj = Instantiate(Prefab_Monster, spawnPosition, Quaternion.identity, parent);

        if (monsterObj == null)
        {
            Debug.LogWarning("몬스터 생성에 실패했습니다.");
            return null;
        }

        int instanceId = GenerateInstanceId();

        if (_createdGameObjectDic.ContainsKey(instanceId))
        {
            Debug.LogWarning($"이미 존재하는 InstanceId입니다. InstanceId : {instanceId}");
            Destroy(monsterObj);
            return null;
        }

        _createdGameObjectDic.Add(instanceId, monsterObj);

        InitGeneratedMonster(instanceId, monsterObj);

        Debug.Log($"몬스터 생성 완료 / InstanceId : {instanceId} / Name : {monsterObj.name}");

        return monsterObj;
    }

    private void InitGeneratedMonster(int instanceId, GameObject monsterObj)
    {
        MG_Monster monster = monsterObj.GetComponent<MG_Monster>();

        if (monster == null)
        {
            Debug.LogWarning($"{monsterObj.name}에 MG_Monster 컴포넌트가 없습니다.");
            return;
        }

        monster.InitMonsterInfo(instanceId);
    }

    public GameObject GetGameObjectCanBeNull(int instanceId)
    {
        if (_createdGameObjectDic.ContainsKey(instanceId) == false)
        {
            Debug.LogWarning($"InstanceId {instanceId} 오브젝트를 찾을 수 없습니다.");
            return null;
        }

        return _createdGameObjectDic[instanceId];
    }

    public void RequestDestroyGameObject(int instanceId)
    {
        GameObject targetObj = GetGameObjectCanBeNull(instanceId);

        if (targetObj == null)
        {
            return;
        }

        _createdGameObjectDic.Remove(instanceId);

        Destroy(targetObj);

        Debug.Log($"오브젝트 제거 완료 / InstanceId : {instanceId}");
    }
}
