using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BulletObjectPool : MonoBehaviour
{
    public static BulletObjectPool instance;

    public class BulletPO
    {
        public GameObject origin;
        public GameObject caseOrigin;
        public List<GameObject> hero_Bullets;
        public List<GameObject> bullet_cases;
        public int hero_bullet_max;

        public BulletPO(GameObject origin, int hero_bullet_max, Transform poolT, GameObject caseOrigin = null)
        {
            this.origin = origin;
            this.hero_bullet_max = hero_bullet_max;
            this.hero_Bullets = new List<GameObject>();
            bullet_cases = new List<GameObject>();
            for (int i = 0; i < hero_bullet_max; i++)
            {
                hero_Bullets.Add(Instantiate(origin, poolT));
            }

            if (caseOrigin)
            {
                for (int i = 0; i < hero_bullet_max * 2; i++)
                {
                    bullet_cases.Add(Instantiate(caseOrigin, poolT));
                }
                this.caseOrigin = caseOrigin;
            }
        }
    }

    Hero hero;
    public BulletPO[] bulletPools;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        hero = Hero.instance;
        bulletPools = new BulletPO[3];
        for (int i = 0; i < bulletPools.Length; i++)
        {
            if(hero.pgs[i].bulletObject)
                bulletPools[i] = new BulletPO(hero.pgs[i].bulletObject, hero.pgs[i].magazineCapacity, transform, hero.pgs[i].bulletCaseOrigin);
        }
        
    }
    public GameObject FindDisabledBullet()
    {
        GameObject res = null;
        
        foreach(var bullet in bulletPools[hero.selectedWeapon].hero_Bullets)
            if(!bullet.activeInHierarchy)
                return bullet;
        return res;
    }
    public void SpawnCase(Quaternion rot)
    {
        Vector3 casePos;
        foreach (var bulletcase in bulletPools[hero.selectedWeapon].bullet_cases)
        {
            if (!bulletcase.activeInHierarchy)
            {
                bulletcase.transform.position = hero.pgs[hero.selectedWeapon].transform.position;
                bulletcase.transform.rotation = rot;
                casePos = bulletcase.transform.position + (rot * -transform.up);
                
                casePos = new Vector3(casePos.x + Random.Range(-0.25f, 0.25f), casePos.y + Random.Range(-0.25f, 0.25f));
                bulletcase.transform.DOJump(casePos, 0.2f, 1, 0.5f);
                bulletcase.transform.DORotateQuaternion(Quaternion.Euler(0, 0, rot.eulerAngles.z + Random.Range(-360f, 360f)),0.5f);
                bulletcase.GetComponent<SpriteRenderer>().DOColor(Color.gray, 0.5f);
                bulletcase.SetActive(true);
                return;
            }
        }
    }
    public bool Shot(Vector3 pos, Quaternion rot, float speed, int bulletnum = 0)
    {
        var newBullet = FindDisabledBullet();
        if (newBullet != null)
        {
            newBullet.transform.position = pos;
            newBullet.transform.rotation = rot;
            newBullet.SetActive(true);
            newBullet.GetComponent<HeroBullet>().SetValue(rot, speed);
            SpawnCase(rot);

            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// 미구현, 사용하지 마시오
    /// </summary>
    public void EnemyShot(Vector3 pos, Quaternion rot, float speed, int bulletnum = 0)
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
