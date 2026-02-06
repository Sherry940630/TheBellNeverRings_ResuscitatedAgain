using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;
    public GameObject debugPanel;

    private GameObject currentPanel;

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (!PauseManager.Instance.IsPaused)
            {
                OpenPauseMenu();
            }
            else
            {
                CloseCurrentPanel();
            }
        }
    }

    public void OpenPauseMenu()
    {
        Debug.Log("Opened Pause Menu.");
        PauseManager.Instance.Pause();
        ShowPanel(pauseMenuPanel);
    }

    public void OpenSettings()
    {
        Debug.Log("Opened Settings.");
        ShowPanel(settingsPanel);
    }

    public void OpenDebug()
    {
        Debug.Log("Opened Debug Menu.");
        ShowPanel(debugPanel);
    }

    private void ShowPanel(GameObject panel)
    {
        //關閉所有面板
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        debugPanel.SetActive(false);

        //開啟指定面板
        if (panel != null)
        {
            Debug.Log(panel + "Is Set Active.");
            panel.SetActive(true);
            currentPanel = panel;
        }
    }

    private void CloseCurrentPanel()
    {
        if (currentPanel != null)
        {
            Debug.Log(currentPanel + "Is Set Inactive.");
            currentPanel.SetActive(false);
        }
        currentPanel = pauseMenuPanel;

        //統一解除暫停
        PauseManager.Instance.Resume();
    }
}

