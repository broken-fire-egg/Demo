using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMove : MonoBehaviour
{
    public float speed;
    public bool ismoving;
    void FixedUpdate()
    {
        if (DialogDisplayer.instance.displaying)
            return;
        KeyCheck();
    }

    public void KeyCheck()
    {
        ismoving = false;
        Vector3 vec3 = new Vector3();




        if (Input.GetKey(KeyCode.W))
        {
            ismoving = true;
            vec3.y += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            ismoving = true;
            vec3.x -= 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            ismoving = true;
            vec3.y -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            ismoving = true;
            vec3.x += 1;
        }
        if (vec3.magnitude == 0)
            ismoving = false;
        gameObject.transform.Translate(vec3.normalized * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
