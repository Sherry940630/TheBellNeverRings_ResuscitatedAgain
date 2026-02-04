using UnityEngine;
using UnityEngine.Rendering;

public class LaplaceTransformAnimation : MonoBehaviour
{
    [Header("Screen FX")]
    public Volume laplaceVolume;
    public float effectDuration = 0.3f;

    private Coroutine routine;

    public void PlayEffect()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(EffectRoutine());
    }

    private System.Collections.IEnumerator EffectRoutine()
    {
        Debug.Log("Playing Laplace Transform screen effect.");

        laplaceVolume.enabled = true;
        laplaceVolume.weight = 1f;
        yield return null;

        float t = 0f;
        float fadeDuration = 0.3f; //想要快就改小

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            laplaceVolume.weight = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        laplaceVolume.weight = 0f;
        laplaceVolume.enabled = false;
    }

}
