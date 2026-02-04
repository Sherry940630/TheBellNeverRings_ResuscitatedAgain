using UnityEngine;

public class ExperienceGems : MonoBehaviour, iCollectibles
{
    [SerializeField, Header("Settings")]
    public GemType gemType;
    public int experienceAmount;
    private float attractionRange = 3f;
    private float attractionSpeed = 5f;

    [SerializeField] private Animator animator;

    private Transform targetPlayer;

    private void Awake()
    {
        SetupGem();
    }
    private void Update()
    {
        FindNearestPlayer();
        AttractToPlayer();
    }

    private void FindNearestPlayer()
    {
        float minDist = Mathf.Infinity;
        targetPlayer = null;

        foreach (var player in PlayerManager.Instance.players)
        {
            float d = Vector2.Distance(transform.position, player.transform.position);
            if (d < minDist && d <= attractionRange)
            {
                minDist = d;
                targetPlayer = player.transform;
            }
        }
    }

    private void AttractToPlayer()
    {
        if (targetPlayer == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPlayer.position,
            attractionSpeed * Time.deltaTime
        );

        float dist = Vector2.Distance(transform.position, targetPlayer.position);
        if (dist <= 0.3f)
        {
            Collect();
        }
    }

    private void SetupGem()
    {
        switch (gemType)
        {
            case GemType.Yellow:
                experienceAmount = 5;
                animator.Play("YellowGemFloating");
                break;

            case GemType.Blue:
                experienceAmount = 10;
                animator.Play("BlueGemFloating");
                break;

            case GemType.Purple:
                experienceAmount = 10;
                animator.Play("PurpleGemFloating");
                break;
        }
    }

    public void Collect()
    {
        if (targetPlayer == null) return;

        //Send to PlayerManager
        PlayerManager.Instance.GiveExperienceToPlayer(targetPlayer.gameObject, experienceAmount);

        Destroy(gameObject);
    }
}
