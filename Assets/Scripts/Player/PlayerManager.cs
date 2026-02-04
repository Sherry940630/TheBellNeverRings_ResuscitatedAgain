using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Header("Player Setup")]
    public List<GameObject> players = new List<GameObject>();
    public static GameObject activePlayer;
    public List<PlayerScriptableObject> playerDataList;
    public List<PlayerRunTimeData> runtimeDataList = new List<PlayerRunTimeData>();

    [Header("Experience System")]
    public PlayerExperience expSystem;

    private CamMovement cam;
    
    private int currentIndex = 0;

    private void Awake()
    {
        Instance = this;
        expSystem = GetComponent<PlayerExperience>();
        /* Auto-find all Players by tag if list is empty. */
        if (players == null || players.Count == 0)
        {
            GameObject[] found = GameObject.FindGameObjectsWithTag("Player");
            players = new List<GameObject>(found);
        }
    }

    private void Start()
    {
        if (players.Count == 0)
        {
            Debug.LogError("No players found!");
            return;
        }

        /* Deactivate all first. */
        foreach (var p in players)
            SetActiveControl(p, false);

        /* Activate the first one. */
        activePlayer = players[currentIndex];
        SetActiveControl(activePlayer, true);

        /* Build runtime data list (from configured ScriptableObjects). */
        runtimeDataList.Clear();
        if (playerDataList != null)
        {
            foreach (var data in playerDataList)
            {
                runtimeDataList.Add(new PlayerRunTimeData(data));
            }
        }

        /* If the runtime list doesn't match player instances, ensure safe size. */
        if (runtimeDataList.Count < players.Count)
        {
            /* Fill missing runtime slots with default entries (avoid index errors). */
            int toAdd = players.Count - runtimeDataList.Count;
            for (int i = 0; i < toAdd; i++)
            {
                /* If we don't have matching ScriptableObject, create placeholder runtime data. */
                runtimeDataList.Add(new PlayerRunTimeData(playerDataList != null && playerDataList.Count > 0 ? playerDataList[0] : ScriptableObject.CreateInstance<PlayerScriptableObject>()));
            }
            Debug.LogWarning($"{nameof(PlayerManager)}: runtimeDataList size was smaller than players. Filled missing entries to avoid out-of-range errors.");
        }

        foreach (var r in runtimeDataList)
        {
            Debug.Log($"{r.baseData.playerName} current health = {r.currentHealth}");
        }

        cam = FindObjectOfType<CamMovement>();
        if (cam != null)
            cam.FollowTarget(activePlayer.transform);
        else
            Debug.LogWarning("CamMovement script not found!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Switch key pressed!");
            SwitchPlayer();
        }

        if (TryGetRuntimeData(activePlayer, out PlayerRunTimeData data))
        {
            if (data.skillCooldownTimer > 0)
                data.skillCooldownTimer -= Time.deltaTime;
        }
    }

    private void SwitchPlayer()
    {
        if (players.Count < 2) return;

        SetActiveControl(activePlayer, false);

        currentIndex = (currentIndex + 1) % players.Count;
        activePlayer = players[currentIndex];

        SetActiveControl(activePlayer, true);

        CamMovement.Instance.FollowTarget(activePlayer.transform);

        Debug.Log($"Switched to {activePlayer.name}");
    }

    private void SetActiveControl(GameObject player, bool isActive)
    {
        var move = player.GetComponent<PlayerMovementScript>();
        var rb = player.GetComponent<Rigidbody2D>();
        if (move != null)
        {
            move.enabled = isActive;

            // 如果是要停用控制，強制將移動向量歸零
            if (!isActive)
            {
                move.moveDir = Vector2.zero;
            }
        }

        // 強制將物理速度歸零，防止角色滑行
        if (!isActive && rb != null)
        {
            rb.velocity = Vector2.zero;
        }

            foreach (WeaponController weapon in player.GetComponentsInChildren<WeaponController>())
            weapon.enabled = isActive;

        var playerCam = player.GetComponentInChildren<Camera>();
        if (playerCam != null) playerCam.enabled = isActive;

        var listener = player.GetComponentInChildren<AudioListener>();
        if (listener != null) listener.enabled = isActive;

        var input = player.GetComponent<PlayerInputSubscription>();
        if (input != null) input.enabled = isActive;
    }

    public void GiveExperienceToPlayer(GameObject collector, int amount)
    {
        if (expSystem == null)
        {
            Debug.LogError("PlayerManager is missing reference to PlayerExperience!");
            return;
        }

        expSystem.AddExperience(collector, amount);
    }

    public bool TryApplyDamage(GameObject player, float damage, out float newHealth)
    {
        newHealth = -1f;

        if (player == null)
        {
            Debug.LogWarning("TryApplyDamage called with null player.");
            return false;
        }

        if (players == null || players.Count == 0)
        {
            Debug.LogWarning("TryApplyDamage: no players registered.");
            return false;
        }

        int playerIndex = players.IndexOf(player);
        if (playerIndex < 0)
        {
            Debug.LogWarning($"TryApplyDamage: player '{player.name}' not found in PlayerManager.players.");
            return false;
        }

        if (runtimeDataList == null || playerIndex >= runtimeDataList.Count)
        {
            Debug.LogWarning($"TryApplyDamage: missing runtime data for player index {playerIndex} ('{player.name}').");
            return false;
        }

        var runtime = runtimeDataList[playerIndex];

        if (runtime.isInvincible)
        {
            Debug.Log($"{player.name} is invincible. Damage ignored.");
            newHealth = runtime.currentHealth;
            return false;
        }

        runtime.currentHealth = Mathf.Max(0f, runtime.currentHealth - damage);

        newHealth = runtime.currentHealth;
        return true;
    }

    public bool TryGetRuntimeData(GameObject player, out PlayerRunTimeData data)
    {
        data = null;
        if (player == null) return false;
        int idx = players.IndexOf(player);
        if (idx < 0 || idx >= runtimeDataList.Count) return false;
        data = runtimeDataList[idx];
        return true;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void RestoreHP(float amount)
    {
        if (activePlayer == null) return;
        if (TryGetRuntimeData(activePlayer, out PlayerRunTimeData data))
        {
            if(data.currentHealth < data.baseData.maxHealth)
            {
                data.currentHealth = Mathf.Min(data.baseData.maxHealth, data.currentHealth + amount);
                if(data.currentHealth > data.baseData.maxHealth)
                {
                    data.currentHealth = data.baseData.maxHealth;
                }
                Debug.Log($"Restored {amount} HP to {activePlayer.name}. Current health: {data.currentHealth}");
            }
            else
            {
                Debug.Log($"{activePlayer.name} is already at max health.");
            }

        }
    }
}
