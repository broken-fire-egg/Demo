using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInvoke : MonoBehaviour
{
    public float lefttime;


    private void Update()
    {
        lefttime -= Time.deltaTime;
        if(lefttime <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        lefttime = 10;
    }
}
