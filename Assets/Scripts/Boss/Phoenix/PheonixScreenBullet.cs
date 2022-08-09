using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheonixScreenBullet : MonoBehaviour
{
    float xpos;
    float ypos;
    public float xspeed = 0;
    public float yspeed = 0;
    public float speedweight = 0.1f;
    private void Start()
    {
    }
    public void Init(Vector3 worldPos)
    {
        worldPos.y += 1.5f;
        Vector2 dir = Hero.instance.transform.position - worldPos;
        dir = dir.normalized;
        Vector3 origin = Camera.main.WorldToScreenPoint(worldPos);
        transform.position = origin;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg, Vector3.forward);
        xpos = origin.x;
        ypos = origin.y;
        xspeed = dir.x * speedweight;
        yspeed = dir.y * speedweight;
    }
    private void Update()
    {
        xpos += xspeed;
        ypos += yspeed;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(xpos, ypos));
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
