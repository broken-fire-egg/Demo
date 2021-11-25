using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofTest : MonoBehaviour
{

    SpriteRenderer sr;
    float alpha;
    public float changeSpeed;

    bool isPlayerInside;
    // Start is called before the first frame update
    void Start()
    {
        alpha = 1;
        isPlayerInside = false;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInside)
            alpha -= changeSpeed;
        else
            alpha += changeSpeed;

        if (alpha > 1)
            alpha = 1;
        else if (alpha < 0)
            alpha = 0;

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isPlayerInside = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isPlayerInside = false;
    }
}
