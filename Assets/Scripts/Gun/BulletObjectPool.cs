using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPool : MonoBehaviour
{
    public static BulletObjectPool instance;
    public GameObject hero_bullet_origin;
    public List<GameObject> hero_Bullets;
    public int hero_bullet_max;
    private void Awake()
    {
        instance = this;
        for(int i= 0; i < hero_bullet_max;i++)
            hero_Bullets.Add(Instantiate(hero_bullet_origin, transform));
    }

    public GameObject FindDisabledBullet()
    {
        GameObject res = null;
        foreach(var bullet in hero_Bullets)
            if(!bullet.activeInHierarchy)
                return bullet;
        return res;
    }

    public void Shot(Vector3 pos, Quaternion rot, float speed, int bulletnum = 0)
    {
        var newBullet = FindDisabledBullet();
        if (newBullet != null)
        {
            newBullet.transform.position = pos;
            newBullet.transform.rotation = rot;
            newBullet.SetActive(true);
            newBullet.GetComponent<HeroBullet>().SetValue(rot, speed);
        }
    }
}
