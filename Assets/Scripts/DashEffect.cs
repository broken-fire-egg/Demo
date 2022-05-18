using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DashEffect : ObjectPooling<DashEffect>
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


    // Update is called once per frame
    void Update()
    {

    }
    public void MakeAfterImage()
    {
        GameObject aii = GetGameObjectFromOP();
        if (aii)
        {
            aii.transform.position = Hero.instance.transform.position;
            aii.SetActive(true);
        }
    }
}
