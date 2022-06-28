using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletObjectPool : MonoBehaviour
{
    //ªË¡¶ø‰∏¡

    static public EnemyBulletObjectPool instance;
    List<GameObject> EnemyBullets;
    GameObject origin;
    public int defaultMax;
    public void Awake()
    {
        instance = this;
        EnemyBullets = new List<GameObject>();
    }
    public void Shot(Vector3 pos, Quaternion rot, float speed)
    {
        var newBullet = FindDisabledBullet();
        if (newBullet != null)
        {
            newBullet.transform.position = pos;
            newBullet.transform.rotation = rot;
            newBullet.SetActive(true);
            newBullet.GetComponent<EnemyBullet>().SetValue(rot, speed);
        }
    }

    public void SetBullet(GameObject origin)
    {
        
        this.origin = origin;
        for (int i = 0;i<defaultMax;i++)
        {
            EnemyBullets.Add(Instantiate(origin,transform));
        }
    }
    public GameObject FindDisabledBullet()
    {
        GameObject res = null;
        foreach (var bullet in EnemyBullets)
            if (!bullet.activeInHierarchy)
                return bullet;
        return res;
    }

}
