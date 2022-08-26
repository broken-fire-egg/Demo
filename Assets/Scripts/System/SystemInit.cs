using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemInit : MonoBehaviour
{
    public Texture2D cursorTexture;
    Texture2D copiedCursorTexture;
    //public Texture2D[] cursor
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        
        if (cursorTexture)
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
    public Texture2D ScaleTexture(Texture2D source, float _scaleFactor)
    {
        if (_scaleFactor == 1f)
        {
            return source;
        }
        else if (_scaleFactor == 0f)
        {
            return Texture2D.blackTexture;
        }

        int _newWidth = Mathf.RoundToInt(source.width * _scaleFactor);
        int _newHeight = Mathf.RoundToInt(source.height * _scaleFactor);



        Color[] _scaledTexPixels = new Color[_newWidth * _newHeight];

        for (int _yCord = 0; _yCord < _newHeight; _yCord++)
        {
            float _vCord = _yCord / (_newHeight * 1f);
            int _scanLineIndex = _yCord * _newWidth;

            for (int _xCord = 0; _xCord < _newWidth; _xCord++)
            {
                float _uCord = _xCord / (_newWidth * 1f);

                _scaledTexPixels[_scanLineIndex + _xCord] = source.GetPixelBilinear(_uCord, _vCord);
            }
        }

        // Create Scaled Texture
        Texture2D result = new Texture2D(_newWidth, _newHeight, source.format, false);
        result.SetPixels(_scaledTexPixels, 0);
        result.Apply();

        return result;
    }




    public float size;
    int currentFrame;
    float frameTimer;

    private void Update()
    {
        copiedCursorTexture = ScaleTexture(cursorTexture, size);
        Cursor.SetCursor(copiedCursorTexture, new Vector2(copiedCursorTexture.width / 2, copiedCursorTexture.height / 2), cursorMode);
    }


}
