using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInvoke : MonoBehaviour
{
    public float lefttimeInit = 10;
    public bool fadeOut;
    public float lefttime;
    public SpriteRenderer spriteRenderer;
    float value;
    private void Awake()
    {
        value = 1f;
    }

    private void Update()
    {
        lefttime -= Time.deltaTime;
        if(lefttime <= 0)
        {
            if(fadeOut)
            {
                StartCoroutine(FadeOut());
            }   
            else
                gameObject.SetActive(false);
        }
    }
    IEnumerator FadeOut()
    {
        while (true)
        {
            value -= Time.deltaTime * 0.1f;
            if (value <= 0)
                break;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, value);
            yield return null;
        }
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        lefttime = lefttimeInit;
        if (fadeOut)
        {
            value = 1f;
            spriteRenderer.color = Color.white;
        }
    }
}
