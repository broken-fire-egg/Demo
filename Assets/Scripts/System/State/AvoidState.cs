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
    public List<Transform> detectedBullets;
    public override State RunCurrentState()
    {
        //sManager.avoiding = true;




        if (t < 5)
            return this;
        else
        {
            t = 0;
            detectedBulletDirection = Vector3.zero;
            b = null;
            prev = Vector3.zero;
            //sManager.avoidCurrentCooltime = sManager.avoidCooltime;
            return nextState[0];
        }
    }
    private void Update()
    {
        if (detectedBullets.Count > 0 && sManagerTest.bulletDetected == true)
        {
            if (prev == Vector3.zero)
            {
                b = detectedBullets[0];
                prev = b.position;
                return;
            }
            else
            {
                detectedBulletDirection = b.position - prev;
                prev = b.position;
            }
            if (detectedBullets[0].gameObject.activeInHierarchy && t < 5)
            {
                
                if (TwoObjectAngle(Hero.instance.gameObject) < TwoObjectAngle(detectedBullets[0].gameObject))
                {
                    gameObject.transform.parent.parent.parent.gameObject.transform.position = Vector3.MoveTowards(transform.position, transform.position + (Vector3)Vector2.Perpendicular(transform.position - Hero.instance.transform.position) * 3, 0.15f);
                }
                else
                {
                    if(TwoObjectAngle(Hero.instance.gameObject) - TwoObjectAngle(detectedBullets[0].gameObject) > 290)
                        gameObject.transform.parent.parent.parent.gameObject.transform.position = Vector3.MoveTowards(transform.position, transform.position + (Vector3)Vector2.Perpendicular(transform.position - Hero.instance.transform.position) * 3, 0.15f);
                    else
                        gameObject.transform.parent.parent.parent.gameObject.transform.position = Vector3.MoveTowards(transform.position, transform.position - (Vector3)Vector2.Perpendicular(transform.position - Hero.instance.transform.position) * 3, 0.15f);
                }
            }
            t++;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
             detectedBullets.Add(collision.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
             detectedBullets.Remove(collision.transform);
        }

    }

    private float TwoObjectAngle(GameObject her)
    {
        Vector3 v = gameObject.transform.position - her.transform.position;
            return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }
}
