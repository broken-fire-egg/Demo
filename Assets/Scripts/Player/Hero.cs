using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hero : MonoBehaviour
{
    static public Hero instance;
    public PlayerGun[] pgs;
    public float debugFloat;

    public Animator animator;
    public float animationSpeed;
    float currentanimationframe = 0f;

    Camera mainCamera;
    HeroMove move;
    public float rotation_degree;
    public float bullet_speed;
    public int selectedWeapon;
    public SpriteRenderer heroRenderer;

    public bool ishand = true;

    private void Awake()
    {
        move = GetComponent<HeroMove>();
        instance = this;
    }
    private void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        animator = GetComponent<Animator>();
        heroRenderer = GetComponent<SpriteRenderer>();
        for(int i=0;i<3;i++)
            pgs[i] = transform.GetChild(0).GetChild(i).GetComponent<PlayerGun>();
    }

    public void Hit()
    {
        Debug.LogError("¾Æ! ¾ÆÆÄ¿ê!!");
    }


    // Update is called once per frame
    void Update()
    {
        if (DialogDisplayer.instance)
            if (DialogDisplayer.instance.displaying)
            return;
        if (Input.GetMouseButtonDown(0) && pgs[selectedWeapon].gameObject.activeInHierarchy)
            GunShot();

        KeyCheck();

        ChangeSpriteAnimation();
    }
    public void ChangeSpriteAnimation()
    {
        //if (!move.onground)
        //{
        //    animator.Play("hero_0_dash_0");
        //    return;
        //}


        UpdateRotationDegree();
        if (currentanimationframe > 1f)
            currentanimationframe -= 1f;
        animator.SetFloat("progress", currentanimationframe);
        currentanimationframe += animationSpeed;

        //Idle
        if (!move.ismoving)
        {
            if (ishand) //Is in town?
            {
                if (rotation_degree > 0 && rotation_degree <= 45f)
                {
                    animator.Play("PlayerIdle0_2");
                }
                else if (rotation_degree > 45 && rotation_degree <= 90f)
                {
                    animator.Play("PlayerIdle0_3");
                }
                else if (rotation_degree > 90 && rotation_degree <= 135f)
                {
                    animator.Play("PlayerIdle0_4");
                }
                else if (rotation_degree > 135 && rotation_degree <= 180f)
                {
                    animator.Play("PlayerIdle0_5");
                }
                else if (rotation_degree > 180 || rotation_degree <= -135f)
                {
                    animator.Play("PlayerIdle0_6");
                }
                else if (rotation_degree > -135f && rotation_degree <= -90f)
                {
                    animator.Play("PlayerIdle0_7");
                }
                else if (rotation_degree > -90 && rotation_degree <= -45f)
                {
                    animator.Play("PlayerIdle0_0");
                }
                else if (rotation_degree > -45 && rotation_degree <= 0f)
                {
                    animator.Play("PlayerIdle0_1");
                }
            }
            else //Is in stage?
            {
                if (rotation_degree > 0 && rotation_degree <= 45f)
                {
                    animator.Play("PlayerIdle1_2");
                }
                else if (rotation_degree > 45 && rotation_degree <= 90f)
                {
                    animator.Play("PlayerIdle1_3");
                }
                else if (rotation_degree > 90 && rotation_degree <= 135f)
                {
                    animator.Play("PlayerIdle1_4");
                }
                else if (rotation_degree > 135 && rotation_degree <= 180f)
                {
                    animator.Play("PlayerIdle1_5");
                }
                else if (rotation_degree > 180 || rotation_degree <= -135f)
                {
                    animator.Play("PlayerIdle1_6");
                }
                else if (rotation_degree > -135f && rotation_degree <= -90f)
                {
                    animator.Play("PlayerIdle1_7");
                }
                else if (rotation_degree > -90 && rotation_degree <= -45f)
                {
                    animator.Play("PlayerIdle1_0");
                }
                else if (rotation_degree > -45 && rotation_degree <= 0f)
                {
                    animator.Play("PlayerIdle1_1");
                }
            }
        }
        else //moving
        {
            if (ishand) //Is in town?
            {
                if (rotation_degree > 0 && rotation_degree <= 45f)
                {
                    animator.Play("PlayerMove0_2");
                }
                else if (rotation_degree > 45 && rotation_degree <= 90f)
                {
                    animator.Play("PlayerMove0_3");
                }
                else if (rotation_degree > 90 && rotation_degree <= 135f)
                {
                    animator.Play("PlayerMove0_4");
                }
                else if (rotation_degree > 135 && rotation_degree <= 180f)
                {
                    animator.Play("PlayerMove0_5");
                }
                else if (rotation_degree > 180 || rotation_degree <= -135f)
                {
                    animator.Play("PlayerMove0_6");
                }
                else if (rotation_degree > -135f && rotation_degree <= -90f)
                {
                    animator.Play("PlayerMove0_7");
                }
                else if (rotation_degree > -90 && rotation_degree <= -45f)
                {
                    animator.Play("PlayerMove0_0");
                }
                else if (rotation_degree > -45 && rotation_degree <= 0f)
                {
                    animator.Play("PlayerMove0_1");
                }
            }
            else //Is in stage?
            {
                if (rotation_degree > 0 && rotation_degree <= 45f)
                {
                    animator.Play("PlayerMove1_2");
                }
                else if (rotation_degree > 45 && rotation_degree <= 90f)
                {
                    animator.Play("PlayerMove1_3");
                }
                else if (rotation_degree > 90 && rotation_degree <= 135f)
                {
                    animator.Play("PlayerMove1_4");
                }
                else if (rotation_degree > 135 && rotation_degree <= 180f)
                {
                    animator.Play("PlayerMove1_5");
                }
                else if (rotation_degree > 180 || rotation_degree <= -135f)
                {
                    animator.Play("PlayerMove1_6");
                }
                else if (rotation_degree > -135f && rotation_degree <= -90f)
                {
                    animator.Play("PlayerMove1_7");
                }
                else if (rotation_degree > -90 && rotation_degree <= -45f)
                {
                    animator.Play("PlayerMove1_0");
                }
                else if (rotation_degree > -45 && rotation_degree <= 0f)
                {
                    animator.Play("PlayerMove1_1");
                }
            }
        }
    }

    public void UpdateRotationDegree()
    {
        Vector3 mouseVector = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseVector.z = transform.position.z;
        mouseVector = (mouseVector - transform.position).normalized;
        rotation_degree = -1 * Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        rotation_degree += 45.0f / 2;
    }


    private void KeyCheck()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            pgs[selectedWeapon].Reload();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeWeapon(2);

    }
    public void ChangeWeapon(int num)
    {
        selectedWeapon = num;
        foreach(var w in pgs)
        {
            if (w != pgs[num])
            {
                w.gameObject.SetActive(false);
                w.GunMagazine.gameObject.SetActive(false);
            }
            else
            {
                w.gameObject.SetActive(true);
                w.GunMagazine.gameObject.SetActive(true);
            }
        }
    }
    public void GunShot()
    {
        if (pgs[selectedWeapon].CanShot())
            pgs[selectedWeapon].GunShot(bullet_speed);
    }



}
