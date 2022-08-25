using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class AssultRifle : PlayerGun
{
    

    //GameObject trash;
    private new void Start()
    {
        base.Start();

        for (int i = 0; i < magazineCapacity; i++)
        {
            bulletList.Add(new BulletInfo(GunMagazine.transform.GetChild(1).gameObject.transform.GetChild(i).gameObject));
            bulletListCopied = new List<BulletInfo>(bulletList);
            OriginbulletList.Add(new BulletInfo(OriginGunMagazine.transform.GetChild(1).gameObject.transform.GetChild(i).gameObject));

            //Debug.Log(OriginGunMagazine.transform.GetChild(1).transform.GetChild(i).gameObject);
        }

        //for (int i = 0; i < magazineCapacity; i++)
        //{
        //    GameObject newGo = Instantiate(bulletUI, bulletUIsParent.transform);
        //    RectTransform rt = newGo.GetComponent<RectTransform>();
        //    rt.anchoredPosition = new Vector2(0, 6 + i * 6);
        //    bulletRects.Add(rt);
        //    bulletUIs.Add(newGo);
        //}
        //bulletRects.Add(bullet)


        SetSequence();
    }
    public override void SetSequence()
    {
        base.SetSequence();
    }


    public override void MagazineMove()
    {
        bulletList[0].rigidbody2D.freezeRotation = false;
        bulletList[0].transform.position = bulletList[bulletList.Count - 1].transform.position;
        bulletList[0].rigidbody2D.AddTorque(20);
        bulletList[0].rigidbody2D.AddForce(Vector2.up * Random.Range(5f, 8f) * 2500);
        bulletList[0].rigidbody2D.AddForce(new Vector2(Random.Range(-2.5f, -1.5f), 0) * 2000);
        bulletList[0].rigidbody2D.gravityScale = 60;
        bulletList[0].image.sprite = BulletUISprites[1];
        bulletList[0].transform.parent = WCanvas;
        bulletList.Remove(bulletList[0]);
    }

    public override void Reload()
    {
        base.Reload();
    }

    public override void ChangeMagazine()
    {
        bulletcount = magazineCapacity;
        bulletList = new List<BulletInfo>(bulletListCopied);
        for (int i = 0; i < bulletList.Count; i++)
        {
            bulletList[i].transform.parent = bulletUIsParent;
            bulletList[i].image.sprite = BulletUISprites[0];
            bulletList[i].rigidbody2D.freezeRotation = true;
            bulletList[i].transform.anchoredPosition = OriginbulletList[i].transform.anchoredPosition;
            bulletList[i].transform.rotation = OriginbulletList[i].transform.rotation;
            bulletList[i].rigidbody2D.velocity = new Vector2(0, 0);
            bulletList[i].rigidbody2D.gravityScale = 0;
        }
    }
    bool ColCheck = false;
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
            ColCheck = true;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            ColCheck = false;
    }
    public override void GunShot(float speed)
    {
        if (ColCheck == false)
            base.GunShot(speed);
    }
}
