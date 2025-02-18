using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] [Range(0, 50)] int poolSize = 5;
    [SerializeField] [Range(0.1f , 30f)] float spawnTimer = 1f;

    GameObject[] pool;

    void Awake()
    {
        populatePool();
    }
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    void populatePool()
    {
        pool = new GameObject[poolSize];
        for(int i =0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(enemyPrefab, transform);
            pool[i].SetActive(false);
        }
    }
    void EnabledObjectInPool()
    {
        for(int i= 0; i < pool.Length; i++)
        {
            if(pool[i].activeInHierarchy == false)
            {
                pool[i].SetActive(true);
                return;
            }

        }
    }
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            EnabledObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}
