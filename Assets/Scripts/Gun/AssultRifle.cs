using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AssultRifle : PlayerGun
{
    private new void Start()
    {
        base.Start();
        for (int i = 0; i < magazineCapacity; i++)
        {
            GameObject newGo = Instantiate(bulletUI, bulletUIsParent.transform);
            RectTransform rt = newGo.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, 6 + i * 6);
            bulletRects.Add(rt);
            bulletUIs.Add(newGo);
        }
        SetSequence();
    }
    public override void SetSequence()
    {
        base.SetSequence();
    }


    public override void MagazineMove()
    {
        bulletUIsParent.DOLocalMoveY(initBulletUIPos.y + (magazineCapacity - bulletcount) * 6, 0.1f);
        bulletRects[bulletcount].DOLocalRotate(new Vector3(0, 0, -90), 0.2f);
        bulletRects[bulletcount].DOLocalMove(new Vector3(75, 200), 0.2f);
    }

    public override void Reload()
    {
        base.Reload();
    }

    public override void ChangeMagazine()
    {
        bulletcount = magazineCapacity;
        for (int i = 0; i < magazineCapacity; i++)
        {
            bulletUIs[i].SetActive(true);
            bulletRects[i].rotation = Quaternion.identity;
            bulletRects[i].anchoredPosition = new Vector2(0, 6 + i * 6);
        }
        bulletUIsParent.anchoredPosition = initBulletUIPos;
    }

    public override void GunShot (float speed)
    {
        base.GunShot(speed);
    }
}
