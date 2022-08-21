using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : PlayerGun
{

    Sequence reloadOnlyOneSeq;
    private new void Start()
    {
        base.Start();
        for (int i = 0; i < magazineCapacity; i++)
        {
            GameObject newGo = Instantiate(bulletUI, bulletUIsParent.transform);
            RectTransform rt = newGo.GetComponent<RectTransform>();
            bulletRects.Add(rt);
            bulletUIs.Add(newGo);
        }
        bulletRects[0].anchoredPosition = new Vector2(20, 0);
        bulletRects[1].anchoredPosition = new Vector2(-20, 0);
        SetSequence();
    }

    public override void ChangeMagazine()
    {
        base.ChangeMagazine();
    }

    public override void GunShot(float speed)
    {
        Debug.Log(bulletcount);
        for(int i=0;i<8;i++)
            BulletObjectPool.instance.Shot(transform.position, Quaternion.AngleAxis(gunAngle + Random.Range(-15f,15f),Vector3.back), speed + Random.Range(-20f, 20f));
        cam.Shake((Hero.instance.transform.position - transform.position).normalized, rebound, 0.05f);
        GunMagazine.transform.DOScale(new Vector2(2.1f, 2.1f), 0.1f);
        GunMagazine.transform.DOScale(new Vector2(1f, 1f), 0.1f);
        GunMagazine.transform.GetChild(1).transform.GetChild(bulletcount % 2).gameObject.SetActive(false);
        bulletcount--;
        MagazineMove();
    }

    public override void MagazineMove()
    {
        bulletRects[bulletcount].GetChild(1).gameObject.SetActive(true);
    }

    public override void Reload()
    {
        if (reloading)
            return;
        reloading = true;

        if (bulletcount == 0)
            reloadSeq.Restart();
        else if (bulletcount == 1)
            reloadOnlyOneSeq.Restart();
        else
            reloading = false;


    }

    public override void SetSequence()
    {
       reloadSeq = DOTween.Sequence();
       reloadSeq.Pause();
        reloadSeq.AppendCallback(() => { GunMagazine.GetComponent<RectTransform>().anchoredPosition = new Vector2(-102, 90); });
        reloadSeq.Append(GunMagazine.GetComponent<RectTransform>().DOLocalMoveY(-800, 0.5f));
        //reloadSeq.AppendCallback(() => { GunMagazine.transform.rotation = Quaternion.Euler(0, 0, 180); });
        reloadSeq.AppendInterval(0.5f);
        //reloadSeq.AppendCallback(() => { GunMagazine.transform.DORotate(new Vector3(0, 0, 0), 0.7f); });
        reloadSeq.Append(GunMagazine.GetComponent<RectTransform>().DOLocalMoveY(-450, 0.5f));
        //reloadSeq.Append(bulletRects[0].DOScale(2, 0.5f));
        //reloadSeq.Join(bulletRects[1].DOScale(2, 0.5f));
        //reloadSeq.AppendCallback(() => { bulletRects[0].gameObject.SetActive(false); bulletRects[1].gameObject.SetActive(false); });
        reloadSeq.AppendInterval(0.5f);
        reloadSeq.AppendCallback(() => { GunMagazine.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true); bulletcount++; });
        reloadSeq.Append(GunMagazine.transform.GetChild(1).transform.GetChild(0).DOScale(1, 0.5f));
        reloadSeq.AppendCallback(() => { GunMagazine.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(true); bulletcount++; });
        reloadSeq.Append(GunMagazine.transform.GetChild(1).transform.GetChild(1).DOScale(1, 0.5f));
        reloadSeq.SetAutoKill(false);
        reloadSeq.AppendCallback(() => { reloading = false; });
        //
        //
        //
        //reloadOnlyOneSeq = DOTween.Sequence();
        //reloadOnlyOneSeq.Pause();
        //reloadOnlyOneSeq.Append(bulletRects[1].DOScale(2, 0.5f));
        //reloadOnlyOneSeq.AppendCallback(() => { bulletRects[1].gameObject.SetActive(false); });
        //reloadOnlyOneSeq.AppendInterval(0.5f);
        //reloadOnlyOneSeq.AppendCallback(() => { bulletRects[1].GetChild(1).gameObject.SetActive(false); bulletRects[1].gameObject.SetActive(true); bulletcount++; });
        //reloadOnlyOneSeq.Append(bulletRects[1].DOScale(1, 0.5f));
        //reloadOnlyOneSeq.AppendCallback(() => { reloading = false; });
        //reloadOnlyOneSeq.SetAutoKill(false);
    }



}
