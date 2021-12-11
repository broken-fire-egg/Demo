using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLTestOP : MonoBehaviour
{
    public GameObject[] redBullet;
    public GameObject[] blueBullet;

    public void OffAll()
    {
        foreach (var bullet in redBullet)
            bullet.SetActive(false);
        foreach (var bullet in blueBullet)
            bullet.SetActive(false);
    }
    public int leftRedBullet { get { 
            int x = 10;
            foreach (var bullet in redBullet)
            {
                if (bullet.activeInHierarchy)
                    x--;
            }
            return x;
                } }
    public int leftBlueBullet
    {
        get
        {
            int x = 10;
            foreach (var bullet in blueBullet)
            {
                if (bullet.activeInHierarchy)
                    x--;
            }
            return x;
        }
    }
    public GameObject FindDisabledBullet(bool isred)
    {
        GameObject res = null;
        if (isred)
        {
            foreach (var bullet in redBullet)
                if (!bullet.activeInHierarchy)
                    return bullet;
        }
        else
        {
            foreach (var bullet in blueBullet)
                if (!bullet.activeInHierarchy)
                    return bullet;
        }
        return res;
    }

    public bool Shot(Vector3 pos, Quaternion rot, bool isred)
    {
        var newBullet = FindDisabledBullet(isred);
        if (newBullet != null)
        {
            newBullet.transform.position = pos;
            newBullet.transform.rotation = rot;
            newBullet.SetActive(true);
            newBullet.GetComponent<Bullet>().SetValue(rot, 250f);
            return true;
        }
        return false;
    }
}
