using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemInit : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    private void Start()
    {
        Cursor.SetCursor(cursorTexture, new Vector2( cursorTexture.width / 2, cursorTexture.height / 2), cursorMode);
    }
    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2, cursorTexture.height / 2), cursorMode);
    }
    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
