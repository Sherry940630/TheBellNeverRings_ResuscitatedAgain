using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    public PauseReason CurrentReason { get; private set; } = PauseReason.None;
    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Pause(PauseReason reason)
    {
        //if (IsPaused) return;
        IsPaused = true;
        CurrentReason = reason;
        Time.timeScale = 0f;
    }

    public void Resume(PauseReason reason)
    {
        if (!IsPaused || CurrentReason != reason) return;

        IsPaused = false;
        CurrentReason = PauseReason.None;
        Time.timeScale = 1f;
    }
}
