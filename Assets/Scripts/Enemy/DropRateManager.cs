using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string itemName;
        public float dropRate;
        public GameObject itemPrefab;
    }

    public List<Drops> drops;

    private void OnEnable()
    {
        EnemyStat.OnEnemyKilled += HandleEnemyKilled;

        BreakableProps.OnPropDestroyed += HandlePropDestroyed;
    }

    private void OnDisable()
    {
        EnemyStat.OnEnemyKilled -= HandleEnemyKilled;
        BreakableProps.OnPropDestroyed -= HandlePropDestroyed;
    }

    private void HandleEnemyKilled(EnemyStat enemy)
    {
        DropItem(enemy.transform.position);
    }

    private void HandlePropDestroyed(BreakableProps prop)
    {
        DropItem(prop.transform.position);
    }

    private void DropItem(Vector3 position)
    {
        float randomValue = Random.Range(0f, 100f);
        List<Drops> possibleDrops = new List<Drops>();

        foreach (Drops rate in drops)
        {
            if (randomValue <= rate.dropRate)
            {
                possibleDrops.Add(rate);
            }
        }
        if(possibleDrops.Count > 0)
        {
            Drops selectedDrop = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
            Instantiate(selectedDrop.itemPrefab, transform.position, Quaternion.identity);
        }
    }
}
