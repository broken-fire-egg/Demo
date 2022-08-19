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
    }
}
