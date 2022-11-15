using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class ToolTip : MonoBehaviour
{
    static public ToolTip instance;
    public TMP_Text tooltipText;
    public RectTransform bgRectTransform;
    float textPaddingSize;
    private void Awake()
    {
        // bgRectTransform = transform.Find("BG").GetComponent<RectTransform>();
        // tooltipText = transform.Find("Text").GetComponent<TextMeshPro>();

        textPaddingSize = 8f;
        instance = this;

    }
    private void Update()
    {
        transform.position =  new Vector3 (Input.mousePosition.x +(bgRectTransform.sizeDelta.x) /2, Input.mousePosition.y - (bgRectTransform.sizeDelta.y) /2);
    }
    private void Start()
    {

    }

    public void ShowToolTip(PlacedObjectTypeSO item)
    {
        ShowToolTip(GenerateTooltipText(item));
    }
    string GenerateTooltipText(PlacedObjectTypeSO placedObjectTypeSO)
    {
        string res = "<align=\"center\"><b>";
        switch (placedObjectTypeSO.rarity)
        {
            case PlacedObjectTypeSO.Rarity.Uncommon:
                res += "<color=green>";
                break;
            case PlacedObjectTypeSO.Rarity.Rare:
                res += "<color=blue>";
                break;
            case PlacedObjectTypeSO.Rarity.Unique:
                res += "<color=purple>";
                break;
            case PlacedObjectTypeSO.Rarity.Legend:
                res += "<color=orange>";
                break;
            default:
                res += "<color=white>";
                break;
        }

        res += placedObjectTypeSO.nameString + "</color></b></align>\n\n<align=\"left\">";

        if (!placedObjectTypeSO.effectText.Equals(""))
            res += placedObjectTypeSO.effectText + "\n\n";

        res += placedObjectTypeSO.description;

        return res;
    }
    public void ShowToolTip(string _text)
    {
        tooltipText.SetText(_text);
        
        Vector2 backgroundSize = new Vector2(tooltipText.renderedWidth + textPaddingSize * 2f, tooltipText.renderedHeight + textPaddingSize * 2f);
        bgRectTransform.sizeDelta = backgroundSize;
        tooltipText.rectTransform.sizeDelta = backgroundSize;
        tooltipText.gameObject.SetActive(true);
        bgRectTransform.gameObject.SetActive(true);
    }
    public void HideToolTip()
    {
        tooltipText.gameObject.SetActive(false);
        bgRectTransform.gameObject.SetActive(false);
    }
}
