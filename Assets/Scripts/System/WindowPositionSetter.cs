using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class WindowPositionSetter : MonoBehaviour
{
    static public WindowPositionSetter instance;
    public static IntPtr hndl;

    public Text text;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern IntPtr FindWindow(System.String className, System.String windowName);
    public struct SimpleRect
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
    }
    public static void SetPosition(int x, int y, int resX = 0, int resY = 0)
    {
        SetWindowPos(hndl, 0, x, y, resX, resY, resX * resY == 0 ? 1 : 0);
    }

    [DllImport("user32.dll")]
    public static extern bool GetWindowRect(IntPtr hwnd, ref SimpleRect rectangle);

#endif

    public int width, height, x, y;
    private void Awake()
    {
        instance = this;
        hndl = FindWindow(null, "UntitledGame");
    }
        SimpleRect Srect = new SimpleRect();
    private void Start()
    {
        Screen.fullScreen = false;
        //SetPosition(Screen.currentResolution.width / 2 - Screen.width, Screen.currentResolution.height / 2 - Screen.height / 2);


    }
    // Update is called once per frame
    private void Update()
    {
        GetWindowRect(hndl, ref Srect);
        if(text)
            text.text =(Screen.currentResolution.width / 2 - Screen.width / 2).ToString() + "," + (Screen.currentResolution.height / 2 - Screen.height / 2).ToString();
        SetPosition(Screen.currentResolution.width / 2 - Screen.width / 2, Screen.currentResolution.height / 2 - Screen.height / 2);
    }
    public IEnumerator Shake(float duration, float magnitude, Vector2 ForceDir = new Vector2())
    {
        float elapsed = 0.0f;
        int minH = 0,
            minW = 0,
            maxH = Screen.currentResolution.height - Screen.height,
            maxW = Screen.currentResolution.width - Screen.width;
        int Xpos = Screen.currentResolution.width / 2 - Screen.width / 2, Ypos = Screen.currentResolution.height / 2 - Screen.height / 2;

        Vector2 windCenter = new Vector2((Srect.Left + Srect.Right) / 2, Screen.currentResolution.height - (Srect.Top + Srect.Bottom) / 2);
        Vector2 Offset = new Vector2();
        Vector2 movedir;
        ForceDir = new Vector2(ForceDir.x, Screen.currentResolution.height - ForceDir.y);
        movedir = (windCenter - ForceDir).normalized;

        while (elapsed < duration)
        {


            Vector2 dir = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
            dir.Normalize();
            dir *= (duration - elapsed) / duration;

            Xpos +=  (int)dir.x  + (int)(movedir * magnitude / 1.2f).x;
            Ypos += (int)dir.y  + (int)(movedir * -magnitude / 1.2f).y;

            Offset += (movedir * magnitude / 1.2f);
            text.text = movedir.x.ToString() + "," + movedir.y.ToString();

            if (Xpos >= maxW)
            {
                Offset = new Vector2(Offset.x - (Xpos - maxW), Offset.y);
                Xpos = maxW;
                movedir = new Vector2(-movedir.x, movedir.y);
            }
            if (Xpos <= minW)
            {
                Offset = new Vector2(Offset.x + (minW - Xpos), Offset.y);
                Xpos = minW;
                movedir = new Vector2(-movedir.x, movedir.y);
            }
            if (Ypos >= maxH)
            {
                Offset = new Vector2(Offset.x, Offset.y - (Ypos - maxH));
                Ypos = maxH;
                movedir = new Vector2(movedir.x, -movedir.y);
            }
            if (Ypos <= minH)
            {

                Offset = new Vector2(Offset.x, Offset.y + (minH - Ypos));
                Ypos = minH;
                movedir = new Vector2(movedir.x, -movedir.y);
            }
            SetPosition(Xpos, Ypos);

            elapsed += Time.deltaTime;
            magnitude *= 0.95f;
            

            yield return null;
        }
        SetPosition(Xpos, Ypos);
    }

}
