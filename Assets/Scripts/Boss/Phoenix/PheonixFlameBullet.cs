using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheonixFlameBullet : MonoBehaviour
{
    public float progress;
    float speed;
    Vector3 dir;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Vector3 pos, Vector3 _dir)
    {
        progress = 0.0f;
        speed = 0.04f;
        transform.position = pos;
        dir = _dir;
        gameObject.SetActive(true);
    }
    private void FixedUpdate()
    {
        if (progress < 1.0f)
            progress += speed;
        else
            gameObject.SetActive(false);
        transform.position = gameObject.transform.position + dir * 0.18f; ;
    }
    // Update is called once per frame
    void Update()
    {
        spriteRenderer.color = new Color(progress, 0, 1 - progress);
    }
}
