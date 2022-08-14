using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkChildrenPosition : MonoBehaviour
{
    public float speed;
    public float minlength;
    public BossState bossState;
    void Start()
    {
        speed = 0.9975f;
        minlength = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        Transform target = null;
        Vector3 value = Vector3.zero;
        for(int i=0;i< transform.childCount;i++)
        {
            target = transform.GetChild(i);
            value = new Vector3(target.localPosition.x, target.localPosition.y, target.localPosition.z);
            if (value.magnitude > minlength)
            {
                value *= speed;
                target.localPosition = value;
            }
        }
    }
    private void OnDisable()
    {
        if (bossState)
            bossState.SendMessageToBoss("resetSCP");
    }
}
