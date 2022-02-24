using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidState : State
{
    Vector3 v;
    public int t = 0;
    public Vector3 detectedBulletDirection = Vector3.zero;
    public Transform b;
    Vector3 prev;

    public override State RunCurrentState()
    {
        sManager.avoiding = true;




        if (t < 5)
            return this;
        else
        {
            t = 0;
            detectedBulletDirection = Vector3.zero;
            b = null;
            prev = Vector3.zero;
            sManager.avoidCurrentCooltime = sManager.avoidCooltime;
            return nextState[0];
        }
    }
    private void Update()
    {
        //Debug.Log(sManager.detectedBullets[0].gameObject.transform.rotation.z);
        if (sManager.currentState == this && sManager.detectedBullets.Count > 0)
        {
            if (prev == Vector3.zero)
            {
                b = sManager.detectedBullets[0];
                prev = b.position;
                return;
            }
            else
            {
                detectedBulletDirection = b.position - prev;
                prev = b.position;
            }
            if (sManager.detectedBullets[0].gameObject.activeInHierarchy && t < 5)
            {
                if (TwoObjectAngle(Hero.instance.gameObject) < TwoObjectAngle(sManager.detectedBullets[0].gameObject))
                {
                    sManager.subject.transform.position = Vector3.MoveTowards(transform.position, transform.position + (Vector3)Vector2.Perpendicular(transform.position - Hero.instance.transform.position) * 3, 0.15f);
                }
                else
                {
                    if(TwoObjectAngle(Hero.instance.gameObject) - TwoObjectAngle(sManager.detectedBullets[0].gameObject) > 290)
                        sManager.subject.transform.position = Vector3.MoveTowards(transform.position, transform.position + (Vector3)Vector2.Perpendicular(transform.position - Hero.instance.transform.position) * 3, 0.15f);
                    else
                        sManager.subject.transform.position = Vector3.MoveTowards(transform.position, transform.position - (Vector3)Vector2.Perpendicular(transform.position - Hero.instance.transform.position) * 3, 0.15f);
                }
            }
            t++;
        }
    }

    private float TwoObjectAngle(GameObject her)
    {
        Vector3 v = gameObject.transform.position - her.transform.position;
            return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }
}
