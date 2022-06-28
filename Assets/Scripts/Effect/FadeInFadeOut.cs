using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInFadeOut : MonoBehaviour
{
    Image img;
    float progress;
    float speed;
    private void Awake()
    {
        img = GetComponent<Image>();
        progress = 0;
        speed = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        if(progress<0)
        {
            //¾ÀÀüÈ¯
        }
        img.color = new Color(progress, progress, progress);
        progress += speed;
        if(progress > 2f)
            speed *= -1;
        
    }
}
