using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPosition : MonoBehaviour
{
    public Vector3 targetPosition;
    public float speed;
    public Vector3 direction;


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
