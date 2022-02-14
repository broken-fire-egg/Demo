using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    Text text;
    RectTransform background;
    private void Awake()
    {

        background = transform.Find("BackGround").GetComponent<RectTransform>();
        text = transform.Find("Text").GetComponent<Text>();

        ShowTooltip("Hello, World!");

    }
    private void Update()
    {
        
    }

    void ShowTooltip(string context) {
        gameObject.SetActive(true);

        text.text = context;
        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(text.preferredWidth + textPaddingSize * 2f, text.preferredHeight + textPaddingSize * 2f);
        background.sizeDelta = backgroundSize;
    }

    void HideTooltip() {

        gameObject.SetActive(false);
    }
}
