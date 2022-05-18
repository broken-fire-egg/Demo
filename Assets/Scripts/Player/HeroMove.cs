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
    private bool dashinput;
    private int dashindex;
    private int dashmaxindex;

    private void Awake()
    {
        dashindex = 0;
        dashmaxindex = 12;
    }
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        dashprogress = 0f;
    }
    void FixedUpdate()
    {
        if (DialogDisplayer.instance)
            if (DialogDisplayer.instance.displaying)
                return;
        if (dashprogress > ((float)1 / dashmaxindex) * dashindex)
        {
            dashindex++;
            DashEffect.instance.MakeAfterImage();
        }
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

        if(dashinput)
        {
            dashinput = false;
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
        if (Input.GetMouseButtonDown(1))
        {
            dashinput = true;
        }


        if (!onground)
        {
            rb2d.velocity = new Vector2(dashdirection.x, dashdirection.y) * dashpower;

            dashprogress += dashspeed;


            if (dashprogress >= 1)
            {

                dashprogress = 0f;
                dashindex = 0;
                rb2d.velocity = Vector2.zero;
                onground = true;
            }
        }
    }
}
