using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHitMask : MonoBehaviour
{
    public SpriteMask sm;
    float progress = 0.001f;
    public float speed;
    bool start;

    private void Update()
    {
        if (start)
        {
            sm.alphaCutoff = progress;
            progress += speed;
            if (progress >= 1)
                Removethis();
        }
    }
    public void RemoveAfterFewSec()
    {
        gameObject.SetActive(true);
        start = true;
        progress = speed;
    }
    public void Removethis()
    {
        Destroy(gameObject);
    }
}
