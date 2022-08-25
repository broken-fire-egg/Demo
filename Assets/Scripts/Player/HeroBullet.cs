using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBullet : Bullet
{

    public float damage;
    public GameObject BarrierMask;

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Barrier"))
        {
            GameObject newGo = Instantiate(BarrierMask, gameObject.transform.position, Quaternion.identity, collision.transform);
            newGo.transform.localScale = new Vector3(0.7f, 0.7f, 1);
            newGo.GetComponent<BarrierHitMask>().RemoveAfterFewSec();
            gameObject.SetActive(false);
            return;
        }
        else if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            var eft = HitEffectObjectPool.instance.GetRestingPoolObject();
            eft.gameObject.SetActive(true);
            eft.gameObject.transform.position = transform.position;
            eft.gameObject.transform.rotation = transform.rotation;
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Barrier"))
        {
            GameObject newGo = Instantiate(BarrierMask, gameObject.transform.position, Quaternion.identity, collision.transform);
            newGo.transform.localScale = new Vector3(0.7f, 0.7f, 1);
            newGo.GetComponent<BarrierHitMask>().RemoveAfterFewSec();
            gameObject.SetActive(false);
            return;
        }
        else if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            var eft = HitEffectObjectPool.instance.GetRestingPoolObject();
            eft.gameObject.SetActive(true);
            eft.gameObject.transform.position = transform.position;
            eft.gameObject.transform.rotation = transform.rotation;
            gameObject.SetActive(false);
        }

    }
}
