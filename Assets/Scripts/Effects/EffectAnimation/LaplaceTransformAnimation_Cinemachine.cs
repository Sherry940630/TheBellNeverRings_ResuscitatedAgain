using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;

public class LaplaceTransformAnimation_Cinemachine : MonoBehaviour
{
    [Header("Screen FX")]
    public Volume laplaceVolume;
    public float effectDuration = 1.5f;

    [Header("Cinemachine FX")]
    public CinemachineImpulseSource impulseSource;

    private Coroutine routine;

    public void PlayEffect()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(EffectRoutine());
    }

    private System.Collections.IEnumerator EffectRoutine()
    {
        Debug.Log("Playing Laplace Transform (Cinemachine) FX.");

        // Trigger impulse camera shake
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }

        // Volume fade-out
        laplaceVolume.weight = 1f;
        laplaceVolume.enabled = true;

        float t = 0f;
        while (t < effectDuration)
        {
            t += Time.deltaTime;
            laplaceVolume.weight = Mathf.Lerp(1f, 0f, t / effectDuration);
            yield return null;
        }

        laplaceVolume.weight = 0f;
        laplaceVolume.enabled = false;
    }
}
