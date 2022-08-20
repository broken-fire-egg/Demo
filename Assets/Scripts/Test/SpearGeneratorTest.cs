using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;

public class SpearGeneratorTest : MonoBehaviour
{
    public GameObject spear;
    public List<GameObject> spears;
    public List<Rigidbody2D> rgs;
    public float delay;
    public float speed;
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    // Start is called before the first frame update
    void Start()
    {
        Process.Start("GameFrame.exe");

        spears = new List<GameObject>();
        for (int i = 0; i < 36; i++)
        {
            var newgo = Instantiate(spear);
            newgo.transform.rotation = Quaternion.Euler(0, 0, i * 10 + 180);
            newgo.transform.position = DegreeToVector2(i * 10) * 15;
            rgs.Add(newgo.GetComponent<Rigidbody2D>());
            spears.Add(newgo);
        }
        Invoke("Fire", delay);
    }
    public void Fire()
    {
        foreach (var rg in rgs)
        {
            rg.AddForce(-rg.transform.position * speed);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
