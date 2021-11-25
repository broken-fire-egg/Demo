using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class WindowPositionSetter : MonoBehaviour
{

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern IntPtr FindWindow(System.String className, System.String windowName);

    public static void SetPosition(int x, int y, int resX = 0, int resY = 0)
    {
        SetWindowPos(FindWindow(null, "UntitledGame"), 0, x, y, resX, resY, resX * resY == 0 ? 1 : 0);
    }
#endif

    public int width, height, x, y;
    public Camera mainCamera;
    public Transform PlayerCenter;
    private void Start()
    {
        Screen.fullScreen = false;
    }
    // Update is called once per frame
    void Update()
    {
       // Vector3 mouseVector = mainCamera.ScreenToWorldPoint(Input.mousePosition);
       //mouseVector.z = transform.position.z;
       //mouseVector = (PlayerCenter.position - mouseVector).normalized;

       // SetPosition(Screen.width / 2, Screen.height / 2);
    }
}
