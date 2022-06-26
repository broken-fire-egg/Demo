using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public float cooltime;

    public override State RunCurrentState()
    {
        if(sManagerTest.IfChange)
        {
            sManagerTest.IfChange = false;
            return nextState[0];
        }
       else if(sManagerTest.bulletDetected)
       {
               return nextState[1];
       }
        return this;
    }
    private void FixedUpdate()
    {
        if (sManagerTest.NowState == StateManagerTest.StateKind.Attack && sManagerTest.IfChange == false)
            //if(sManager.currentState == this)
            sManager.subject.transform.position = Vector3.MoveTowards(transform.position, Hero.instance.transform.position, 0.05f);
            
    }
    private void Update()
    {
        cooltime += Time.deltaTime;
        if (Vector2.Distance(transform.position, Hero.instance.transform.position) < 1.0f && transform.parent.GetChild(2).GetComponent<AttackState>().cooltime <= 0)
        {
            sManagerTest.IfChange = true;
        }
        else
        {
            sManagerTest.IfChange = false;
        }
        if (Vector2.Distance(transform.position, Hero.instance.transform.position) > 4f && cooltime >= 5.0f)
        {
            sManagerTest.NowState = StateManagerTest.StateKind.Idle;
            cooltime = 0f;
        }
    }
}
