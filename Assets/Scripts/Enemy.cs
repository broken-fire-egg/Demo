using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;
    public float maxhp;
    public Transform hpBar;

    void Hit(float damage)
    {
        hp -= damage;
        if (hp <= 0)
            Destroy(gameObject);
        hpBar.localScale = new Vector3(1, hp/maxhp, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 dir = Hero.instance.transform.position - transform.position;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerBullet"))
        {
            Hit(collision.gameObject.GetComponent<HeroBullet>().damage);
            collision.gameObject.SetActive(false);
        }
    }
}
