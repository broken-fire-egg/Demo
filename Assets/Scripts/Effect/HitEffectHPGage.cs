using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitEffectHPGage : MonoBehaviour
{
    Image img;
    public Material mat;
    RectTransform rectTransform;
    public BossState boss;
    public float val;
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        val = 69;
        mat = img.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (val > 255f)
            val = 255f;
        if (val > 69)
            val -= 4f;
        else
            val = 69f;
        img.color = new Color(245f / 255f, val / 255f, val / 255f);
        rectTransform.sizeDelta = new Vector2(boss.GetHPGage(), rectTransform.sizeDelta.y);
    }
}
