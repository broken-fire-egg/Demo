using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkChildrenPosition : MonoBehaviour
{
    public float speed;
    public float minlength;
    public float lefttime;
    public BossState bossState;
    void Start()
    {
        speed = 0.995f;
        minlength = 6f;
        lefttime = 9;
    }

    // Update is called once per frame
    void Update()
    {
        lefttime -= Time.deltaTime;
        Transform target;
        Vector3 value;
        for(int i=0;i< transform.childCount;i++)
        {
            target = transform.GetChild(i);
            value = new Vector3(target.localPosition.x, target.localPosition.y, target.localPosition.z);
            if (lefttime > 3f)
            {
                if (value.magnitude > minlength)
                {
                    value *= speed;
                    target.localPosition = value;
                }
            }
            else
            {
                value *= 1f / speed;
                target.localPosition = value;
            }
        }
    }
    private void OnDisable()
    {
        lefttime = 9;
        if (bossState)
            bossState.SendMessageToBoss("resetSCP");
    }
}
