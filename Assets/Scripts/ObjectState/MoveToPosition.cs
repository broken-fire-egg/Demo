using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPosition : MonoBehaviour
{
    public Vector3 targetPosition;
    public float speed = 0.015f;
    public Vector3 direction;

    private void Awake()
    {
        speed = 0.015f;

    }
    public void Init(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
        direction = (targetPosition - transform.position).normalized * speed;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction);
    }
}
