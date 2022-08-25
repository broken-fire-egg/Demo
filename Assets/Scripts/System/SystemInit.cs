using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemInit : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        if(cursorTexture)
            Cursor.SetCursor(cursorTexture, new Vector2( cursorTexture.width / 2, cursorTexture.height / 2), cursorMode);
    }
    void OnMouseEnter()
    {
        if (cursorTexture)
            Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2, cursorTexture.height / 2), cursorMode);
    }
    void OnMouseExit()
    {
        if (cursorTexture)
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
