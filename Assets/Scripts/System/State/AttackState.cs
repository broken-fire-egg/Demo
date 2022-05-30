using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public bool attack = false;
    public float cooltime;
    public float delay;
    private void Update()
    {
        cooltime -= Time.deltaTime;
    }
    public override State RunCurrentState()
    {
        if (cooltime <= 0)
        {
            Debug.Log("°ø°ÝÇÔ");
            cooltime = delay;
            return nextState[0];
        }
        else
        {
            return this;
        }
    }
}
