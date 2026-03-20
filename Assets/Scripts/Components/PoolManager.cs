using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
  public static PoolManager Instance {  get; private set; }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [SerializeField] private List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDicitionary;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        poolDicitionary = new Dictionary<string, Queue<GameObject>>();
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                objectPool.Enqueue(obj);
            }
            poolDicitionary.Add(pool.tag, objectPool);
        }

    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDicitionary.ContainsKey(tag))
        {
            return null;
        }

        GameObject objectToSpawn;

        if (poolDicitionary[tag].Count > 0)
        {
            objectToSpawn = poolDicitionary[tag].Dequeue();
        }
        else
        {
            Pool pool = pools.Find(p => p.tag == tag);
            objectToSpawn = Instantiate(pool.prefab);
            objectToSpawn.transform.SetParent(transform);
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        
        return objectToSpawn;
    }

    public void ReturnToPool(string tag, GameObject obj)
    {
        if (!poolDicitionary.ContainsKey(tag)) return;

        obj.SetActive(false);
        poolDicitionary[tag].Enqueue(obj);
    }
}

