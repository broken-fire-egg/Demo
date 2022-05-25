using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DashEffect : ObjectPooling<AfterImage>
{
    public GameObject gunObject;
    static public DashEffect instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    new void Start()
    {

        base.Start();


    }
    public void MakeAfterImage(SpriteRenderer sr, Vector3 pos, Quaternion rot)
    {
        PoolObject po = GetRestingPoolObject();
        if (po.gameObject)
        {

            po.component.targetsr = sr;
            po.gameObject.transform.position = pos;
            po.gameObject.transform.rotation = rot;
            po.gameObject.SetActive(true);
        }
    }
}
