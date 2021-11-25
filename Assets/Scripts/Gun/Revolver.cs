using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Revolver : PlayerGun
{
    public float angle = 0f;
    int k = 0;
    Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    new void Start()
    {
        base.Start();
        for (int i = 0; i < magazineCapacity; i++)
        {
            GameObject newGo = Instantiate(bulletUI, bulletUIsParent.transform);
            RectTransform rt = newGo.GetComponent<RectTransform>();
            rt.anchoredPosition += DegreeToVector2(360 - (i * 60) + 30) * 45;
            bulletRects.Add(rt);
            bulletUIs.Add(newGo);
        }

        angle = 0;
        SetSequence();
    }
    public override void GunShot(float speed)
    {
        base.GunShot(speed);
    }
    public override void MagazineMove()
    {
        angle -= 60;
        if (angle == -360)
            angle = 0;
        bulletRects[bulletcount].GetChild(1).gameObject.SetActive(true);
        bulletUIsParent.DORotate(new Vector3(0,0, angle), 0.25f);
    }
    public override void ChangeMagazine()
    {
        base.ChangeMagazine();
    }
    public override void Reload()
    {
        base.Reload();
    }
    public override void SetSequence()
    {
        reloadSeq = DOTween.Sequence();
        reloadSeq.Pause();
        reloadSeq.SetAutoKill(false);
        reloadSeq.Append(bulletRects[0].DOScale(2, 0.3f));
        for (int i = 1; i < magazineCapacity; i++)
            reloadSeq.Join(bulletRects[i].DOScale(2, 0.3f));
        reloadSeq.AppendCallback(() => {
            for (int i = 0; i < magazineCapacity; i++) { 
                bulletUIs[i].SetActive(false); 
                bulletRects[i].GetChild(1).gameObject.SetActive(false);
            }
            bulletUIsParent.rotation = Quaternion.identity;
        });
        reloadSeq.AppendInterval(0.3f);
        for (int i = 0; i < magazineCapacity; i++)
        {
            reloadSeq.AppendCallback(GameobjectSetActiveTrue);
            reloadSeq.Append(bulletRects[i].DOScale(1, 0.3f));
            //if (i != magazineCapacity - 1)
            //{
            //    angle -= 60;
            //    reloadSeq.Append(bulletUIsParent.DORotate(new Vector3(0, 0, angle), 0.25f));
            //}
        }
        reloadSeq.AppendCallback(() => { ChangeMagazine();  reloading = false; angle = 0; k = 0; });
    }
    public void GameobjectSetActiveTrue()
    {
        bulletUIs[k].SetActive(true);
        k++;
    }
}
