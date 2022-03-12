using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;
    public float maxhp;
    public float knockBackTimer;
    public Transform hpBar;

    float timer;
    public bool a = false;
    Vector2 test;

    void Hit(float damage)
    {
        hp -= damage;
        if (hp <= 0)
            Destroy(gameObject);
        if(hpBar)
         hpBar.localScale = new Vector3(1, hp / maxhp, 1);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (a == true && knockBackTimer <= timer)
            backStop();
        //Vector3 dir = Hero.instance.transform.position - transform.position;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            backStop();
            GetComponent<Rigidbody2D>().AddForce(collision.gameObject.GetComponent<Rigidbody2D>().velocity * 100f);
            timer = 0;
            Hit(collision.gameObject.GetComponent<HeroBullet>().damage);
            collision.gameObject.SetActive(false);
            a = true;
        }
    }

    void backStop()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        a = false;
    }
}
