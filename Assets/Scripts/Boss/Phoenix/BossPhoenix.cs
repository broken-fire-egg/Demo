using System.Collections;
using System.Collections.Generic;

using System.IO.Pipes;
using System.Diagnostics;

using System.Runtime.InteropServices;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Threading;

public class BossPhoenix : BossState
{

    public GameObject bullet_Screen;
    public GameObject bullet_Normal;
    public GameObject bullet_Flame;

    public class ScreenBulletPool : ObjectPooling<PheonixScreenBullet>
    {
        public void Init(GameObject go)
        {
            defaultCap = 10;
            origin = go;
            base.Start();
        }
    }
    ScreenBulletPool bullet_Screen_Pool;


    public class NormalBulletpool : ObjectPooling<Transform>
    {
        public void Init(GameObject go)
        {
            defaultCap = 25;
            origin = go;
            base.Start();
        }
    }
    NormalBulletpool bullet_Normal_Pool;

    public class FlameBulletPool : ObjectPooling<PheonixFlameBullet>
    {
        public void Init(GameObject go)
        {
            defaultCap = 40;
            origin = go;
            base.Start();
        }
    }
    FlameBulletPool bullet_Flame_Pool;
    public class BurnAroundBulletPool : ObjectPooling<PheonixFlameBullet>
    {
        public GameObject center;
        public void Init(GameObject go)
        {
            center = new GameObject("bac", typeof(TurnAround), typeof(ShrinkChildrenPosition),typeof(MoveToPosition),typeof(DisableInvoke));
            center.SetActive(false);
            defaultCap = 60;
            origin = go;
            base.Start();
            ResetPosition();
        }
        public void ResetPosition()
        {
            center.transform.position = transform.position;
            int i;
            float angle;
            foreach (var po in poolObjects)
            {
                po.gameObject.transform.SetParent(center.transform);
                i = po.gameObject.transform.GetSiblingIndex();
                angle = (360f / defaultCap) * i;
                po.gameObject.transform.localPosition = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                po.gameObject.transform.localPosition *= 30;
                po.gameObject.transform.localRotation = Quaternion.Euler(0, 0, angle);
                po.gameObject.SetActive(true);
                po.spriteRenderer.sortingOrder = 10;
            }
        }
    }
    BurnAroundBulletPool bullet_ba_Pool;
    public float bulletSpeed;

    NamedPipeServerStream namedPipeServerStream;
    StreamString streamString;
    bool connection = false;
    bool winBomb = false;
    int status = 0;
    Vector2 bombPos = Vector2.zero;
    Thread serverReadThread;


    new void Start()
    {
        Application.runInBackground = true;
        bullet_Screen_Pool = gameObject.AddComponent<ScreenBulletPool>();
        bullet_Normal_Pool = gameObject.AddComponent<NormalBulletpool>();

        

        bullet_Flame_Pool = new GameObject("bfp").AddComponent<FlameBulletPool>();
        bullet_ba_Pool = gameObject.AddComponent<BurnAroundBulletPool>();
        bullet_Screen_Pool.Init(bullet_Screen);
        bullet_Normal_Pool.Init(bullet_Normal);
        bullet_Flame_Pool.Init(bullet_Flame);
        bullet_ba_Pool.Init(bullet_Normal);
        base.Start();
#if (!UNITY_EDITOR)
        serverReadThread = new Thread(ServerThread_Read);
        serverReadThread.Start();
#endif
    }
    private void OnApplicationQuit()
    {
#if (!UNITY_EDITOR)
        serverReadThread.Abort();
#endif
    }
    private void Update()
    {

        Debug.Log(Vector3.Distance(Hero.instance.transform.position, transform.position));

        switch (status)
        {
            case 1 :
                status = 0;
                break;
            case 2:
                StartCoroutine(WindowPositionSetter.instance.Shake(4f, 300, bombPos));
                status = 0;
                break;
            default:
                break;
        }
    }
    public void StartPattern()
    {
        StartCoroutine(CheckQueue());
    }
    bool BurnArounding;
    protected override IEnumerator GetPattern(int n)
    {
        Debug.Log(n);
        switch (n)
        {
            case 0:
                return ScreenShot();
            case 1:
                return FlameShot();
            case 2:
                if (!winBomb)
                {
                    return SetBomb();
                }
                else
                    return FlameShot();
            case 3:
                if (!BurnArounding)
                {
                    BurnArounding = true;
                    return BurnAround();
                }
                else
                    return ScreenShot();
            default:
                return null;
        }
    }

    WaitForSeconds ss_BeforeDelay = new WaitForSeconds(0.8f);
    WaitForSeconds ss_AfterDelay = new WaitForSeconds(3f);

    public void DecreaseAttackCount()
    {
        animator.SetInteger("LeftAttack", animator.GetInteger("LeftAttack") - 1);
    }
    IEnumerator ScreenShot()
    {
        animator.SetTrigger("UIAttack");
        yield return ss_BeforeDelay;
        var newbullet = bullet_Screen_Pool.GetRestingPoolObject();
        newbullet.gameObject.SetActive(true);
        newbullet.component.Init(transform.position);
        yield return ss_AfterDelay;
    }

    WaitForSeconds fs_BeforeDelay = new WaitForSeconds(1.5f);
    WaitForSeconds fs_FireDelay = new WaitForSeconds(0.05f);
    WaitForSeconds fs_AfterDelay = new WaitForSeconds(3f);
    Vector3 bulletdir;
    IEnumerator FlameShot()
    {
        animator.SetTrigger("Attack");
        animator.SetInteger("LeftAttack", 3);
        yield return fs_BeforeDelay;
        for (int i = 0; i < 30; i++)
        {
            var po = bullet_Flame_Pool.GetRestingPoolObject();
            if (po.component == null)
                Debug.Log("huh?");
            bulletdir = (Hero.instance.transform.position - transform.position).normalized;
            po.component.Init(transform.position + bulletdir * 1.5f, bulletdir);
            yield return fs_FireDelay;
        }

        yield return fs_AfterDelay;

    }
    public float teleport_target_distance;
    public float teleport_threshold;
    public void TeleportToPlayer()
    {
        Vector3 heroPos = Hero.instance.transform.position;
        float distance = Vector3.Distance(heroPos, transform.position);
        if(teleport_threshold < distance)
        {
            Vector3 dir = transform.position - heroPos;
            dir.Normalize();
            rigid.velocity = -dir;
            dir *= teleport_target_distance;
            rigid.MovePosition(heroPos + dir);
        }


    }
    WaitForSeconds sb_BeforeDelay = new WaitForSeconds(0.2f);
    WaitForSeconds sb_AfterDelay = new WaitForSeconds(3f);
    void ServerThread_Read()
    {
        namedPipeServerStream = new NamedPipeServerStream("BombPipe", PipeDirection.In);
        namedPipeServerStream.WaitForConnection();
        UnityEngine.Debug.Log("Client Detected");
        connection = true;
        streamString = new StreamString(namedPipeServerStream);

        while (connection)
        {
            string message = streamString.ReadString();
            UnityEngine.Debug.Log("Recived: " + message);
            if (message != null)
            {
                if (message.Contains("Bomb Planted!"))
                {
                    UnityEngine.Debug.Log(message);

                    message = message.Replace("Bomb Planted!", "");
                    bombPos = new Vector2(int.Parse(message.Split(',')[0]), int.Parse(message.Split(',')[1]));
                    status = 1;
                }
                if (message == "Bomb Exploded!")
                {
                    status = 2; //행동 queue 역할을 수행함
                    //connection = false;
                    winBomb = false;
                }
                if (message == "Bomb Defused!")
                {
                    //connection = false;
                    winBomb = false;
                }
            }
        }
    }
    IEnumerator SetBomb()
    {
        winBomb = true;
        yield return sb_BeforeDelay;
        Debug.Log("Bomb Planted");
        
        yield return sb_AfterDelay;
    }

    WaitForSeconds ba_BeforeDelay = new WaitForSeconds(0.2f);
    WaitForSeconds ba_AfterDelay = new WaitForSeconds(3f);

    IEnumerator BurnAround()
    {
        animator.SetTrigger("Attack");
        animator.SetInteger("LeftAttack", 1);
        yield return ba_BeforeDelay;

        yield return ba_AfterDelay;
    }
    public override void SendMessageToBoss(string msg)
    {
        base.SendMessageToBoss(msg);
        switch(msg)
        {
            case "resetSCP":
                bullet_ba_Pool.ResetPosition();
                BurnArounding = false;
                break;
        }

    }
}
