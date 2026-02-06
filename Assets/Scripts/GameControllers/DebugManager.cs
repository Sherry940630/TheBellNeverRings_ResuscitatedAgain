using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // 必須引用新的 Input System

public class DebugManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pausePanel;

    [Header("References")]
    public BossCountdown bossCountdown;

    private bool isPaused = false;

    // 如果你有設定 Input Action Asset，可以在這裡引用
    // 這裡示範直接偵測鍵盤，或是你可以透過 PlayerInput 組件呼叫
    void Update()
    {
        // 偵測新版 Input System 的 ESC 按鍵
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // 暫停遊戲
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f; // 恢復遊戲
            pausePanel.SetActive(false);
        }
    }

    // 按鈕點擊後呼叫的功能
    public void Debug_ForceBossTimerZero()
    {
        if (bossCountdown != null)
        {
            bossCountdown.ForceBossSpawn();
            TogglePause(); // 執行後自動解除暫停回歸遊戲
        }
    }
}