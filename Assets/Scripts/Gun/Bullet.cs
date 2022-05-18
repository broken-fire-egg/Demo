using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb2d;
    private void Awake()
    {
        if(rb2d == null)
            rb2d = GetComponent<Rigidbody2D>();
    }
    public void SetValue(Quaternion dir, float speed)
    {
        rb2d.AddForce(transform.right * speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<MLTestWall>(out var a))
        {
            gameObject.SetActive(false);
        }
    }
}
