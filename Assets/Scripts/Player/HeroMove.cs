using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMove : MonoBehaviour
{
    public float speed;
    public bool ismoving;
    public Rigidbody2D rb2d;

    public float dashpower;
    public float dashspeed;
    public float dashprogress;
    public bool onground;
    Vector3 dashdirection;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        dashprogress = 0f;
    }
    void FixedUpdate()
    {
        if (DialogDisplayer.instance.displaying)
            return;
        KeyCheck();
        
    }

    public void KeyCheck()
    {
        if (!onground)
            return;
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

        if(Input.GetMouseButton(1))
        {
            Debug.Log("Trydash");
            if (vec3.magnitude != 0)
            {
                dashdirection = vec3.normalized;
                onground = false;
            }
        }


        gameObject.transform.Translate(vec3.normalized * speed);




    }

    // Update is called once per frame
    void Update()
    {
        if(!onground)
        {
            rb2d.velocity = new Vector2(dashdirection.x, dashdirection.y) * dashpower;

            dashprogress += dashspeed;
            if (dashprogress >= 1)
            {

                dashprogress = 0f;
                rb2d.velocity = Vector2.zero;
                onground = true;
            }
        }
    }
}
