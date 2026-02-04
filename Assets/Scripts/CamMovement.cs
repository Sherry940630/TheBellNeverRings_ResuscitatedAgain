using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    public static CamMovement Instance { get; private set; }

    private CinemachineVirtualCamera vcam;
    private Coroutine zoomCoroutine;

    private void Awake()
    {
        Instance = this;
        vcam = FindObjectOfType<CinemachineVirtualCamera>();
        if (vcam == null)
            Debug.LogError("No Cinemachine Virtual Camera found in scene!");
    }

    public void FollowTarget(Transform target)
    {
        if (vcam == null) return;

        vcam.Follow = target;
        vcam.LookAt = target;   //讓鏡頭朝向角色
    }

    // ===== Boss 戰拉遠鏡頭 =====
    public void SmoothZoom(float targetValue, float duration)
    {
        if (vcam == null) return;

        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(ZoomRoutine(targetValue, duration));
    }

    private IEnumerator ZoomRoutine(float targetValue, float duration)
    {
        float time = 0f;

        // === 選一個用 ===
        bool isOrtho = vcam.m_Lens.Orthographic;

        float startValue = isOrtho
            ? vcam.m_Lens.OrthographicSize
            : vcam.m_Lens.FieldOfView;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            float value = Mathf.Lerp(startValue, targetValue, t);

            if (isOrtho)
                vcam.m_Lens.OrthographicSize = value;
            else
                vcam.m_Lens.FieldOfView = value;

            yield return null;
        }

        //確保最後值精準
        if (isOrtho)
            vcam.m_Lens.OrthographicSize = targetValue;
        else
            vcam.m_Lens.FieldOfView = targetValue;
    }
}
