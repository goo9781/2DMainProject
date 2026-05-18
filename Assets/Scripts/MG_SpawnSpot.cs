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
}
