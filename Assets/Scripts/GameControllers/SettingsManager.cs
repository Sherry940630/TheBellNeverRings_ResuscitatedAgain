using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Range(0f, 1f)]
    public float volumeMultiplier = 1f;

    private void Awake()
    {
        Instance = this;
    }

    public void SetVolume(float value)
    {
        volumeMultiplier = value;
    }
}
