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
    public RectTransform bulletUIsParent;
    public List<GameObject> bulletUIs;

    public GameObject GunMagazine;
    public GameObject OriginGunMagazine;
    public Transform instancePosition;

    public GameObject bulletObject;

    public Vector3 Spriteoffset;
    protected CameraMove cam;
    protected Camera mainCamera;
    protected SpriteRenderer weaponRenderer;
    protected List<RectTransform> bulletRects;
    protected Sequence reloadSeq;
    protected Transform gunpoint;
    protected Transform WCanvas;
    Vector3 originalGunpoint;
    Vector3 previousMousePosition;

    public class BulletInfo
    {
        public GameObject gameobject;
        public Rigidbody2D rigidbody2D;
        public RectTransform transform;

        public BulletInfo(GameObject gameobject)
        {
            this.gameobject = gameobject;
            this.rigidbody2D = gameobject.GetComponent<Rigidbody2D>();
            this.transform = gameobject.GetComponent<RectTransform>();
        }
    }


    public List<BulletInfo> bulletList = new List<BulletInfo>();

    public List<BulletInfo> bulletListCopied = new List<BulletInfo>();
    public List<BulletInfo> OriginbulletList = new List<BulletInfo>();






    public void Start()
    {
        previousMousePosition = Vector3.zero;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cam = mainCamera.GetComponent<CameraMove>();
        bulletRects = new List<RectTransform>();
        weaponRenderer = GetComponent<SpriteRenderer>();
        gunpoint = transform.GetChild(0);
        originalGunpoint = gunpoint.localPosition;
        PlayerCenter = transform.parent;
        WCanvas = GameObject.Find("WCanvas").transform;
        GunMagazine = Instantiate(OriginGunMagazine, WCanvas);
        if(GunMagazine)
            initBulletUIPos = GunMagazine.GetComponent<RectTransform>().anchoredPosition;

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

    public void MakeDashEffect()
    {
        DashEffect.instance.MakeAfterImage(weaponRenderer, transform.position, transform.rotation);
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
        if (!BulletObjectPool.instance)
            return;
        if(!BulletObjectPool.instance.Shot(gunpoint.position,transform.rotation,speed))
            return;
        bulletcount--;
        cam.Shake((PlayerCenter.position - transform.position).normalized, rebound, 0.05f);
        MagazineMove();
    }
    public virtual void MagazineMove() { }
    public virtual void SetSequence() {
        reloadSeq = DOTween.Sequence();
        reloadSeq.Pause();
        reloadSeq.Append(GunMagazine.GetComponent<RectTransform>().DOMoveX(-100, 0.5f));
        reloadSeq.AppendInterval(0.5f);
        reloadSeq.AppendCallback(ChangeMagazine);
        reloadSeq.AppendCallback(() => { GunMagazine.GetComponent<RectTransform>().anchoredPosition = new Vector2( initBulletUIPos.x, initBulletUIPos.y - 145); });
        reloadSeq.Append(GunMagazine.GetComponent<RectTransform>().DOMoveY(145, 0.5f));
        reloadSeq.SetAutoKill(false);
        reloadSeq.AppendCallback(() => { reloading = false; });
    }
}
