using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb2d;
    private void Start()
    {

    }
    public void SetValue(Quaternion dir, float speed)
    {
        rb2d.AddForce(dir * Vector2.right * speed);
    }
}
