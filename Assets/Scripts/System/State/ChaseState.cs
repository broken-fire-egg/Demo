using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{

    
    public override State RunCurrentState()
    {
        if(sManager.playerInrange)
        {
            return nextState[0];
        }
        else if(sManager.bulletDetected)
        {
            if(sManager.avoidCurrentCooltime <= 0)
                return nextState[1];
        }
        return this;
    }
    private void FixedUpdate()
    {
        if(sManager.currentState == this)
            sManager.subject.transform.position = Vector3.MoveTowards(transform.position, Hero.instance.transform.position, 0.05f);
    }
    private void Update()
    {
        if (Vector2.Distance(transform.position, Hero.instance.transform.position) < 1.8f)
            sManager.playerInrange = true;
        else
            sManager.playerInrange = false;
    }
}
