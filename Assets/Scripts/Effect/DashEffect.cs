using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DashEffect : ObjectPooling<AfterImage>
{
    static public DashEffect instance;
    private void Awake()
    {
        instance = this;
    }
    new void Start()
    {
        base.Start();
    }
    public void MakeAfterImage(SpriteRenderer sr, Vector3 pos, Quaternion rot)
    {
        PoolObject po = GetRestingPoolObject();
        if(po != null)
        if (po.gameObject)
        {
            po.component.targetsr = sr;
            po.gameObject.transform.position = pos;
            po.gameObject.transform.rotation = rot;
            po.component.Init();
            po.gameObject.SetActive(true);
        }
    }
}
