using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform heroTransform;

     Vector3 target;
     Vector3 offset;
     Vector3 refVel;
     Vector3 mousePos;
     Vector3 shakeOffset;
     Vector3 shakeVector;

    public float cameraDist;
    public float smoothTime;
    public bool shaking;
    private float shakeMag;
    private float shakeTimeEnd;

    // Start is called before the first frame update
    void FixedUpdate()
    {
        SetCurrentMousePosition();
        UpdateShake();
        UpdateTargetPosition();
        UpdateCameraPoisition();
    }

    private void UpdateTargetPosition()
    {
        Vector3 mouseOffset = mousePos * cameraDist;
        Vector3 ret = heroTransform.position + mouseOffset;
        ret += shakeOffset;
        ret.z = -10;
        target = ret;
    }

    public void UpdateCameraPoisition()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime);
    }
    public void SetCurrentMousePosition()
    {
        
        Vector2 vec2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        vec2 *= 2;
        vec2 -= Vector2.one;
        float max = 0.9f;
        if (Mathf.Abs(vec2.x) > max || Mathf.Abs(vec2.y) > max)
            vec2.Normalize();
        mousePos = vec2;

    }
    void UpdateShake()
    {
        if (!shaking || Time.time > shakeTimeEnd)
        {
            shaking = false; //set shaking false when the shake time is up
            shakeOffset = Vector3.zero; //return zero so that it won't effect the target
            return;
        }
        Vector3 tempOffset = shakeVector;
        tempOffset *= shakeMag; //find out how far to shake, in what direction
        shakeOffset = tempOffset;
    }
    public void Shake(Vector3 direction, float magnitude, float length)
    { //capture values set for where it's called
        shaking = true; //to know whether it's shaking
        shakeVector = direction; //direction to shake towards
        shakeMag = magnitude; //how far in that direction
        shakeTimeEnd = Time.time + length; //how long to shake
    }
}
