using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInFadeOut : MonoBehaviour
{
    Image img;
    RawImage rawImage;
    public float delay;
    public bool onlyFadeOut;
    float progress;
    float speed;
    bool start;
    bool isRaw;
    private void Awake()
    {
        if (!TryGetComponent<Image>(out img))
        {
            isRaw = true;
            rawImage = GetComponent<RawImage>();
        }
        if (onlyFadeOut)
            progress = 2;
        else
            progress = 0;
        speed = 0.01f;
    }
    private void Start()
    {
        Invoke("TurnOn", delay);
    }
    void TurnOn()
    {
        start = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!start)
            return;
        if (progress < 0)
        {
            //¾ÀÀüÈ¯
        }
        if (isRaw)
            rawImage.color = new Color(1, 1, 1, progress);
        else
            img.color = new Color(1, 1, 1, progress);
        progress += speed;
        if (progress > 2f)
            speed *= -1;

    }
}
