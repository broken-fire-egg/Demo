using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerGun : MonoBehaviour
{
    public int magazineCapacity;
    public int bulletcount;

    public Vector2 initBulletUIPos;
    public float rebound;
    public float weaponDist;
    protected float gunAngle;
    public bool reloading;

    private Transform PlayerCenter;
    public GameObject bulletUI;
    public RectTransform magazineUI;
    public RectTransform bulletUIsParent;
    public List<GameObject> bulletUIs;

    public Vector3 Spriteoffset;

    protected CameraMove cam;
    protected Camera mainCamera;
    protected SpriteRenderer weaponRenderer;
    protected List<RectTransform> bulletRects;
    protected Sequence reloadSeq;
    protected Transform gunpoint;
    Vector3 originalGunpoint;
    Vector3 previousMousePosition;
    public void Start()
    {
        previousMousePosition = Vector3.zero;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cam = mainCamera.GetComponent<CameraMove>();
        bulletRects = new List<RectTransform>();
        weaponRenderer = GetComponent<SpriteRenderer>();
        initBulletUIPos = bulletUIsParent.anchoredPosition;
        gunpoint = transform.GetChild(0);
        originalGunpoint = gunpoint.localPosition;
        PlayerCenter = transform.parent;
    }
    public void Update()
    {
        if (DialogDisplayer.instance)
            if (DialogDisplayer.instance.displaying)
            return;
        RotateGun();
    }
    public void RotateGun()
    {
        Vector3 mouseVector = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseVector.z = transform.position.z;

        

        mouseVector = (mouseVector - PlayerCenter.position).normalized;
        gunAngle = -1 * Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;

        
        transform.rotation = Quaternion.AngleAxis(gunAngle, Vector3.back);


        weaponRenderer.sortingOrder = Hero.instance.heroRenderer.sortingOrder - 1;

        if (gunAngle > 0)
        {
            weaponRenderer.sortingOrder = Hero.instance.heroRenderer.sortingOrder + 1;
        }
        
        transform.localPosition = mouseVector * weaponDist;
        transform.localPosition += Spriteoffset;

        if (Mathf.Abs(gunAngle) < 90f)
        {
            gunpoint.localPosition = new Vector3(originalGunpoint.x, Mathf.Abs(originalGunpoint.y), originalGunpoint.z);
            weaponRenderer.flipY = false;
        }
        else
        {
            weaponRenderer.flipY = true;
            gunpoint.localPosition = new Vector3(originalGunpoint.x, -Mathf.Abs(originalGunpoint.y), originalGunpoint.z); weaponRenderer.flipY = true;
        }
    }


    public bool CanShot()
    {
        if (bulletcount <= 0)
            return false;
        if (reloading)
            return false;

        return true;
    }
    public virtual void Reload()
    {
        if (reloading)
            return;
        reloading = true;
        reloadSeq.Restart();
    }
    public virtual void ChangeMagazine() { bulletcount = magazineCapacity; }
    public virtual void GunShot(float speed)
    {
        BulletObjectPool.instance.Shot(gunpoint.position,transform.rotation,speed);
        bulletcount--;
        cam.Shake((PlayerCenter.position - transform.position).normalized, rebound, 0.05f);
        MagazineMove();
    }
    public virtual void MagazineMove() { }
    public virtual void SetSequence() {
        reloadSeq = DOTween.Sequence();
        reloadSeq.Pause();
        reloadSeq.AppendCallback(() => { magazineUI.anchoredPosition = new Vector2(-70, 145); });
        reloadSeq.Append(magazineUI.DOLocalMoveX(1000, 0.5f));
        reloadSeq.AppendInterval(0.5f);
        reloadSeq.AppendCallback(() => { magazineUI.anchoredPosition = new Vector2(-70, -120); });
        reloadSeq.AppendCallback(ChangeMagazine);
        reloadSeq.Append(magazineUI.DOMoveY(145, 0.5f));
        reloadSeq.SetAutoKill(false);
        reloadSeq.AppendCallback(() => { reloading = false; });
    }
}
