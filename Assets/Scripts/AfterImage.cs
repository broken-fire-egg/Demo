using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public float progress = 0;
    public float fadeTime = 0.02f;

    private SpriteRenderer targetsr;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    private void Awake()
    {
        targetsr = Hero.instance.GetComponent<SpriteRenderer>();
        sr = GetComponent<SpriteRenderer>();
        
    }
    void Start()
    {
    }
    private void OnEnable()
    {
        sr.sprite = targetsr.sprite;
        sr.flipX = targetsr.flipX;
        sr.flipY = targetsr.flipY;
        progress = 0;
        
    }
    // Update is called once per frame
    void Update()
    {
        sr.color = new Color(0.5f, 0.5f, 0.5f, (1f - progress) / 2);
        if (progress > 1)
        {
            gameObject.SetActive(false);
            progress = 0;
        }
        progress += fadeTime;
    }
}
