using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class AssultRifle : PlayerGun
{
    public List<GameObject> bulletList = new List<GameObject>();
    public List<GameObject> OriginbulletList = new List<GameObject>();
    //GameObject trash;
    private new void Start()
    {
        base.Start();
        for (int i = 0; i < GunMagazine.transform.GetChild(1).gameObject.transform.childCount; i++)
        {
            bulletList.Add(GunMagazine.transform.GetChild(1).gameObject.transform.GetChild(i).gameObject);
            OriginbulletList.Add(OriginGunMagazine.transform.GetChild(1).transform.GetChild(i).gameObject);
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
        bulletList[0].transform.position = bulletList[bulletList.Count - 1].transform.position;
        bulletList[0].gameObject.GetComponent<Rigidbody2D>().AddTorque(15000);
        bulletList[0].gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * Random.Range(4, 7) * 10000);
        bulletList[0].gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-10, 10), 0) * 1000);
        bulletList[0].gameObject.GetComponent<Rigidbody2D>().gravityScale = 300;
        bulletList.Remove(bulletList[0]);
    }

    public override void Reload()
    {
        base.Reload();
    }

    public override void ChangeMagazine()
    {
        bulletcount = magazineCapacity;
        bulletList.Clear();
        for (int i = 0; i < GunMagazine.transform.GetChild(1).gameObject.transform.childCount; i++)
        {
            bulletList.Add(GunMagazine.transform.GetChild(1).gameObject.transform.GetChild(i).gameObject);
            bulletList[i].GetComponent<Rigidbody2D>().freezeRotation = true;
            bulletList[i].GetComponent<Rigidbody2D>().freezeRotation = false;
            bulletList[i].GetComponent<RectTransform>().anchoredPosition = OriginbulletList[i].GetComponent<RectTransform>().anchoredPosition;
            bulletList[i].gameObject.transform.rotation = OriginbulletList[i].gameObject.transform.rotation;
            bulletList[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            bulletList[i].GetComponent<Rigidbody2D>().gravityScale = 0;

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
