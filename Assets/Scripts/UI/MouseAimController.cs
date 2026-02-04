using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAimController : MonoBehaviour
{
    public static MouseAimController Instance;

    public Vector2 MouseWorldPos { get; private set; }
    public Vector2 AimDirection { get; private set; }

    [Header("Cursor Visual")]
    public Transform cursorVisual;

    private Camera mainCam;

    private void Awake()
    {
        Instance = this;
        mainCam = Camera.main;
    }

    private void Update()
    {
        UpdateMouseWorldPos();
        UpdateCursorVisual();
    }

    private void UpdateMouseWorldPos()
    {
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(mainCam.transform.position.z);
        MouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreen);
    }

    public Vector2 GetAimDirFrom(Vector3 from)
    {
        AimDirection = (MouseWorldPos - (Vector2)from).normalized;
        return AimDirection;
    }

    private void UpdateCursorVisual()
    {
        if (cursorVisual != null)
            cursorVisual.position = MouseWorldPos;
    }
}