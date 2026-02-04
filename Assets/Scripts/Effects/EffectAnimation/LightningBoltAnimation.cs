using UnityEngine;

public class LightningBoltAnimation : MonoBehaviour
{
    LineRenderer lr;

    float lifetime = 0.1f;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void Setup(Vector3 start, Vector3 end)
    {
        int segments = 6;
        lr.positionCount = segments;

        lr.SetPosition(0, start);
        lr.SetPosition(segments - 1, end);

        // Create zig-zag effect
        for (int i = 1; i < segments - 1; i++)
        {
            float t = i / (float)(segments - 1);
            Vector3 pos = Vector3.Lerp(start, end, t);

            pos += new Vector3(
                Random.Range(-0.25f, 0.25f),
                Random.Range(-0.25f, 0.25f),
                0
            );

            lr.SetPosition(i, pos);
        }

        Destroy(gameObject, lifetime);
    }
}
