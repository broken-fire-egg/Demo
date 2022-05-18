using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamaga : BossState
{
    public GameObject bullet;
    public float bulletSpeed;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    new void Start()
    {
        base.Start();
        EnemyBulletObjectPool.instance.SetBullet(bullet);
    }
    protected override IEnumerator GetPattern(int n)
    {
        Debug.Log(n);
        switch(n)
        {
            case 0:
                return TripleContinousShot();
            case 1:
                return FanShot();
            case 2:
                return ReflectBarrior();
            case 3:
                return BarriorPlusShot();
            default:
                return null;
        }
    }



    IEnumerator TripleContinousShot()
    {
        Debug.Log("TripleContinousShot");
        int ran = 0;
        switch (ran)
        {
            case 0:
                NextPattern = FanShot();
                break;
        }
        Vector2 dir = (Hero.instance.transform.position - transform.position).normalized;
        animator.Play("Attack");
        EnemyBulletObjectPool.instance.Shot(transform.position, Quaternion.AngleAxis(-Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.back), bulletSpeed);
        yield return new WaitForSeconds(0.2f);
        animator.Play("Attack");
        dir = (Hero.instance.transform.position - transform.position).normalized;
        EnemyBulletObjectPool.instance.Shot(transform.position, Quaternion.AngleAxis(-Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.back), bulletSpeed);
        yield return new WaitForSeconds(0.2f);
        animator.Play("Attack");
        dir = (Hero.instance.transform.position - transform.position).normalized;
        EnemyBulletObjectPool.instance.Shot(transform.position, Quaternion.AngleAxis(-Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.back), bulletSpeed);
        yield return new WaitForSeconds(0.25f);
    }

    IEnumerator FanShot()
    {
        Debug.Log("fanshot");
        var wait = new WaitForSeconds(0.5f);
        var fwait = new WaitForSeconds(0.05f);
        int n = 0;
        while (n <= 360)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) ;
            animator.Play("Attack");
            EnemyBulletObjectPool.instance.Shot(transform.position, Quaternion.AngleAxis(270f-n, Vector3.back), bulletSpeed);
            yield return fwait;
            n += 15;
        }
        
        yield return wait;
    }

    IEnumerator ReflectBarrior()
    {
        Debug.Log("ReflectBarrior");
        animator.Play("Defence");



        yield return new WaitForSeconds(2f);
    }
    IEnumerator BarriorPlusShot()
    {
        Vector2 dir = (Hero.instance.transform.position - transform.position).normalized;



        Debug.Log("BarriorPlusShot");
        animator.Play("Defence");


        yield return new WaitForSeconds(0.5f);
        animator.Play("DefenceAttack");
        dir = (Hero.instance.transform.position - transform.position).normalized;
        EnemyBulletObjectPool.instance.Shot(transform.position, Quaternion.AngleAxis(-Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.back), bulletSpeed);
        yield return new WaitForSeconds(0.4f);
        animator.Play("DefenceAttack");
        dir = (Hero.instance.transform.position - transform.position).normalized;
        EnemyBulletObjectPool.instance.Shot(transform.position, Quaternion.AngleAxis(-Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.back), bulletSpeed);
        yield return new WaitForSeconds(0.4f);
        animator.Play("DefenceAttack");
        dir = (Hero.instance.transform.position - transform.position).normalized;
        EnemyBulletObjectPool.instance.Shot(transform.position, Quaternion.AngleAxis(-Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.back), bulletSpeed);
        yield return new WaitForSeconds(2f);

    }
}
