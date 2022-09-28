using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentListSizeAutoSetter : MonoBehaviour
{
    RectTransform rect;
    List<RectTransform> contentList;
    // Start is called before the first frame update
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        contentList = new List<RectTransform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            contentList.Add(transform.GetChild(i).GetComponent<RectTransform>());
        }
    }
    float GetTotalHeight()
    {
        float res = 0;
        foreach (var t in contentList)
        {
            if(t.gameObject.activeInHierarchy)
                res += t.sizeDelta.y;
        }
        return res;
    }


    // Update is called once per frame
    void Update()
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, GetTotalHeight());
    }
}
