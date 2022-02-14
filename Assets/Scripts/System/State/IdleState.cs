using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    
    public override State RunCurrentState()
    {
        if (sManager.playerInsight)
        {
            return nextState[0];
        }
        else
        {
            return this;
        }
    }


    private void Update()
    {
       // Debug.Log(Vector2.Distance(transform.position, Hero.instance.transform.position));
        if (Vector2.Distance(transform.position, Hero.instance.transform.position) < 6f)
            sManager.playerInsight = true;
        else
            sManager.playerInsight = false;
    }
}
