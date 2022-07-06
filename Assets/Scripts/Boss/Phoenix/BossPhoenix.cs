using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossPhoenix : BossState
{
    
    public GameObject bullet_Screen;
    public GameObject bullet_Normal;
    public GameObject bullet_Flame;

    public class ScreenBulletPool : ObjectPooling<PheonixScreenBullet>
    {
        public void Init(GameObject go)
        {
            defaultCap = 10;
            origin = go;
            base.Start();
        }
    }
    ScreenBulletPool bullet_Screen_Pool;


    public class NormalBulletpool : ObjectPooling<Transform>
    {
        public void Init(GameObject go)
        {
            defaultCap = 25;
            origin = go;
            base.Start();
        }
    }
    NormalBulletpool bullet_Normal_Pool;

    public class FlameBulletPool : ObjectPooling<PheonixFlameBullet>
    {
        public void Init(GameObject go)
        {
            defaultCap = 40;
            origin = go;
            base.Start();
        }
    }
    FlameBulletPool bullet_Flame_Pool;

    public float bulletSpeed;
    new void Start()
    {

        bullet_Screen_Pool = gameObject.AddComponent<ScreenBulletPool>();
        bullet_Normal_Pool = gameObject.AddComponent<NormalBulletpool>();
        bullet_Flame_Pool = gameObject.AddComponent<FlameBulletPool>();

        bullet_Screen_Pool.Init(bullet_Screen);

        bullet_Normal_Pool.Init(bullet_Normal);

        bullet_Flame_Pool.Init(bullet_Flame);

        base.Start();
    }

    public void StartPattern()
    {
        StartCoroutine(CheckQueue());
    }
    protected override IEnumerator GetPattern(int n)
    {
        Debug.Log(n);
        switch (n)
        {
            case 0:
                // return ScreenShot();
                return FlameShot();
            case 1:
                return FlameShot();
            case 2:
                // return SetBomb();
                return FlameShot();
            case 3:
                // return BurnAround();
                return FlameShot();
            default:
                return null;
        }
    }

    WaitForSeconds ss_BeforeDelay = new WaitForSeconds(0.2f);
    WaitForSeconds ss_AfterDelay = new WaitForSeconds(0.2f);

    public void DecreaseAttackCount()
    {
        animator.SetInteger("LeftAttack", animator.GetInteger("LeftAttack") - 1);
    }
    IEnumerator ScreenShot()
    {
        yield return ss_BeforeDelay;
        bullet_Screen_Pool.GetRestingPoolObject();
        yield return ss_AfterDelay;
    }

    WaitForSeconds fs_BeforeDelay = new WaitForSeconds(1.5f);
    WaitForSeconds fs_FireDelay = new WaitForSeconds(0.05f);
    WaitForSeconds fs_AfterDelay = new WaitForSeconds(1f);
    Vector3 bulletdir;
    IEnumerator FlameShot()
    {
        animator.SetTrigger("Attack");
        animator.SetInteger("LeftAttack", 3);
        yield return fs_BeforeDelay;
        for(int i = 0; i < 30; i++)
        {
            var po = bullet_Flame_Pool.GetRestingPoolObject();
            if (po.component == null)
                Debug.Log("huh?");
            bulletdir = (Hero.instance.transform.position - transform.position).normalized;
            po.component.Init(transform.position + bulletdir * 1.5f, bulletdir);
            yield return fs_FireDelay;
        }

        yield return fs_AfterDelay;

    }

    WaitForSeconds sb_BeforeDelay = new WaitForSeconds(0.2f);
    WaitForSeconds sb_AfterDelay = new WaitForSeconds(0.2f);

    IEnumerator SetBomb()
    {
        yield return sb_BeforeDelay;

        yield return sb_AfterDelay;
    }

    WaitForSeconds ba_BeforeDelay = new WaitForSeconds(0.2f);
    WaitForSeconds ba_AfterDelay = new WaitForSeconds(0.2f);

    IEnumerator BurnAround()
    {
        yield return ba_BeforeDelay;

        yield return ba_AfterDelay;
    }
}
