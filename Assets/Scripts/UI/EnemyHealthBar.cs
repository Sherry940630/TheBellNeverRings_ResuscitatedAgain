using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject[] hpBlocks;

    public void SetHealth(float current, float max)
    {
        int visibleBlocks = Mathf.CeilToInt(current);

        for (int i = 0; i < hpBlocks.Length; i++)
        {
            hpBlocks[i].SetActive(i < visibleBlocks);
        }
    }
}

