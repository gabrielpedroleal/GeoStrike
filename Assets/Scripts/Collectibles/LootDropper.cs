using System.Collections.Generic;
using UnityEngine;

public class LootDropper : MonoBehaviour
{
    [System.Serializable]
    public class LootItem
    {
        public string poolTag;
        [UnityEngine.Range(0, 100)] public float dropWeight;
    }

    [SerializeField] private List<LootItem> lootTable;
    [SerializeField, Range(0, 100)] private float dropCahnce = 30f;

    public void DropItem()
    {
        if (Random.Range(0f, 100f) > dropCahnce) return;

        float totalWeight = 0;
        foreach (var item in lootTable) totalWeight += item.dropWeight;

        float randomValue = Random.Range(0, totalWeight);
        float currentWeightSum = 0;

        foreach(var item in lootTable)
        {
            currentWeightSum += item.dropWeight;
            if(randomValue <= currentWeightSum)
            {
                PoolManager.Instance.SpawnFromPool(item.poolTag, transform.position, Quaternion.identity);
                break;
            }
        }
    }


}

