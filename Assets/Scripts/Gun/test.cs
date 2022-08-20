using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class test : MonoBehaviour
{
    public GameObject a;
    public GameObject[] bullet;
    int i = 0;

    private bool PhoneBool = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (PhoneBool == false)
            {
                gameObject.transform.DOMoveY(300, 1.0f).SetEase(Ease.OutBounce);
                PhoneBool = true;
            }
            else
            {
                gameObject.transform.DOMoveY(-865, 1.0f).SetEase(Ease.Flash);
                PhoneBool = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }
}
