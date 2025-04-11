using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    public float kSpawnRadius = 10f;
    public List<long> kMonsterUIDList;

    public int kSpawnLimit = 5;
    private int mSpawnCount = 0;

    void Start()
    {
        while (mSpawnCount < kSpawnLimit)
        {
            SpawnMonster();
        }

        StartCoroutine(CoSpawn());
    }
    
    IEnumerator CoSpawn()
    {
        yield return new WaitUntil(() => mSpawnCount < kSpawnLimit);
        yield return new WaitForSeconds(5f);
        SpawnMonster();
    }

    void SpawnMonster()
    {
        Vector3 spawnPoint = GetRandomPoint();

        int index = Random.Range(0, kMonsterUIDList.Count);
        MonsterData data = Mng.table.FindMonsterData(kMonsterUIDList[index]);
        Monster prefab = Resources.Load<Monster>(data.prefabPath);
        Monster newMonster = Instantiate(prefab, spawnPoint, Quaternion.identity);
        newMonster.OnDeath += OnMonsterDeath;
        newMonster.Init(transform, data);

        mSpawnCount++;
    }

    Vector3 GetRandomPoint()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 direction = Random.insideUnitCircle * kSpawnRadius;
            Vector3 randomPoint = transform.position + new Vector3(direction.x, 0, direction.y);

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, kSpawnRadius, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return transform.position;
    }

    void OnMonsterDeath(Monster monster)
    {
        monster.OnDeath -= OnMonsterDeath;  // 오브젝트 풀링시 유효한 것, 현재는 의미 없음
        mSpawnCount--;
        Debug.Log(mSpawnCount);
    }
}
