using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLTestBG : MonoBehaviour
{
    public float speed;
    public SpriteRenderer sr;
    void FixedUpdate()
    {
        float r = sr.color.r + speed > 1f ? 1f : sr.color.r + speed;
        float g = sr.color.g + speed > 1f ? 1f : sr.color.g + speed;
        float b = sr.color.b + speed > 1f ? 1f : sr.color.b + speed;

        sr.color = new Color(r, g, b);

    }
}
