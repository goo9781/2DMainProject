using UnityEngine;

public enum MGSpawnSpotType
{
    None = 0,
    Harvest,
    DropItem,
    Dialogue,
    Monster
}

public enum MGStartSpawnType
{
    None = 0,
    OnAwake,
    OnEnable,
    OnRange,
}

public class MG_SpawnSpot : MonoBehaviour
{
    [SerializeField] private MGSpawnSpotType _spawnSpotType;
    [SerializeField] private MGStartSpawnType _startSpawnType;

    [SerializeField] private string _spawnObjectDataId;
    [SerializeField] private Collider2D Collider_OnSpawnStart;

    [SerializeField] private GameObject Prefab_SpawnObject;
    [SerializeField] private Transform Transform_SpawnPosition;

    private bool _isSpawned;

    private void Awake()
    {

        if(_startSpawnType == MGStartSpawnType.OnAwake)
        {
            StartSpawn();
        }
    }

    private void Start()
    {

        if(_startSpawnType == MGStartSpawnType.OnEnable)
        {
            StartSpawn();
        }

        if(Collider_OnSpawnStart != null)
        {
            Collider_OnSpawnStart.enabled = (_startSpawnType == MGStartSpawnType.OnRange);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") == true)
        {
            StartSpawn();
        }


    }

    private void StartSpawn()
    {
        if (_isSpawned)
        {
            return;
        }

        _isSpawned = true;


        switch (_spawnSpotType)
        {
            case MGSpawnSpotType.Dialogue:
                MGUIManager.Instance.OpenDialogueUI(_spawnObjectDataId);
                this.gameObject.SetActive(false);
                break;

            case MGSpawnSpotType.DropItem:
                SpawnDropItem();
                this.gameObject.SetActive(false);
                break;

            case MGSpawnSpotType.Monster:
                SpawnMonster();
                this.gameObject.SetActive(false);
                break;


        }
    }

    private void SpawnDropItem()
    {

        if (Prefab_SpawnObject == null)
        {
            return;
        }

        Vector3 spawnPosition = transform.position;

        if (Transform_SpawnPosition != null)
        {
            spawnPosition = Transform_SpawnPosition.position;
        }

        GameObject spawnObject = Instantiate(Prefab_SpawnObject, spawnPosition, Quaternion.identity);

    }

    private void SpawnMonster()
    {
        if (MGGameObjectManager.Inst == null)
        {
            Debug.LogWarning("MGGameObjectManager가 없습니다.");
            return;
        }

        if (MGGameDataManager.Inst == null)
        {
            Debug.LogWarning("MGGameDataManager가 없습니다.");
            return;
        }

        MGMonsterData monsterData = MGGameDataManager.Inst.GetMonsterData(_spawnObjectDataId);

        if (monsterData == null)
        {
            Debug.LogWarning($"몬스터 데이터를 찾을 수 없습니다. DataId : {_spawnObjectDataId}");
            return;
        }

        GameObject monsterPrefab = null;

        if (string.IsNullOrEmpty(monsterData.PrefabPath) == false)
        {
            monsterPrefab = Resources.Load<GameObject>(monsterData.PrefabPath);
        }

        if (monsterPrefab == null)
        {
            monsterPrefab = Prefab_SpawnObject;
        }

        if (monsterPrefab == null)
        {
            Debug.LogWarning($"생성할 몬스터 프리팹이 없습니다. DataId : {_spawnObjectDataId}");
            return;
        }

        Vector3 spawnPosition = transform.position;

        if (Transform_SpawnPosition != null)
        {
            spawnPosition = Transform_SpawnPosition.position;
        }

        MG_Monster monster = MGGameObjectManager.Inst.RequestSpawnMonster(monsterPrefab, spawnPosition);

        if (monster == null)
        {
            Debug.LogWarning("몬스터 생성에 실패했습니다.");
            return;
        }

        monster.InitMonsterData(_spawnObjectDataId);
    }
}
