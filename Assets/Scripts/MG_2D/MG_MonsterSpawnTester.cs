using UnityEngine;

public class MG_MonsterSpawnTester : MonoBehaviour
{
    [SerializeField] private Transform Transform_SpawnPoint;
    [SerializeField] private KeyCode _spawnKey = KeyCode.M;

    private void Update()
    {
        if (Input.GetKeyDown(_spawnKey))
        {
            SpawnMonster();
        }
    }

    private void SpawnMonster()
    {
        if (MGGameObjectManager.Inst == null)
        {
            Debug.LogWarning("MGGameObjectManager가 없습니다.");
            return;
        }

        Vector3 spawnPosition = transform.position;

        if (Transform_SpawnPoint != null)
        {
            spawnPosition = Transform_SpawnPoint.position;
        }

        MGGameObjectManager.Inst.RequestSpawnMonster(spawnPosition);
    }
}
