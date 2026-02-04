using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public Text title;
    public Image icon;
    public Image targetPlayerIcon;
    private Button button;

    private UpgradeSO upgrade;
    private GameObject targetPlayer;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogWarning($"{name}: No Button component found on the same GameObject.");
        }
        else
        {
            button.onClick.RemoveListener(OnClick);
            button.onClick.AddListener(OnClick);
            Debug.Log($"{name}: Button listener wired in Awake.");
        }

        if (EventSystem.current == null)
            Debug.LogWarning($"{name}: No EventSystem found in scene ¡X UI clicks won't work.");

        var canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
            Debug.LogWarning($"{name}: No parent Canvas found. Button must be inside a Canvas to receive clicks.");

        var gr = canvas != null ? canvas.GetComponent<GraphicRaycaster>() : null;
        if (gr == null)
            Debug.LogWarning($"{name}: Canvas missing GraphicRaycaster ¡X UI clicks will not be detected.");
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(OnClick);
    }

    public void Setup(UpgradeSO u, GameObject target)
    {
        targetPlayer = target;
        upgrade = u;
        title.text = u != null ? u.upgradeName : "NULL";
        icon.sprite = u != null ? u.icon : null;
        gameObject.SetActive(true);
        Debug.Log($"{name}: Setup called for upgrade '{(u != null ? u.upgradeName : "NULL")}'.");
    }

    public void OnClick()
    {
        Debug.Log($"{name}: UpgradeButton clicked: {(upgrade != null ? upgrade.upgradeName : "NULL upgrade")}");
        if (upgrade == null)
        {
            Debug.LogWarning($"{name}: Upgrade reference is null. Did you call Setup(...) before showing this button?");
            return;
        }
        if (UpgradeManager.Instance == null)
        {
            Debug.LogWarning($"{name}: UpgradeManager.Instance is null.");
            return;
        }
        UpgradeManager.Instance.ChooseUpgrade(upgrade, targetPlayer);
    }
    public void Clear()
    {
        upgrade = null;
        title.text = "";
        icon.sprite = null;
    }
}
