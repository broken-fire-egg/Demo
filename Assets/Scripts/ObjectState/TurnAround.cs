using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAround : MonoBehaviour
{
    public float speed;
    float value;
    private void Start()
    {
        value = 0;
        speed = 10f;
    }
    void Update()
    {
        if (value > 360)
            value -= 360;
        transform.localRotation = Quaternion.Euler(0, 0, value);
        value += speed * Time.deltaTime;
    }
}
