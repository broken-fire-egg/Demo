using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public bool playerInrange;
    public bool playerInsight;
    public State currentState;
    public SpriteRenderer statusimage;
    public GameObject subject;
    public Rigidbody2D rigid;
    public bool bulletDetected;
    public bool avoiding;
    public float avoidCurrentCooltime;
    public float avoidCooltime;

    private void Start()
    {
        subject = transform.parent.gameObject;
        rigid = subject.GetComponent<Rigidbody2D>();
    }
    public void CalculateCooltime()
    {
        avoidCurrentCooltime -= Time.deltaTime;
    }
    private void Update()
    {
        //RunStateMachine();
        CalculateCooltime();
    }
   // private void RunStateMachine()
   // {
   //     State nextState = currentState?.RunCurrentState();
   //
   //     if(nextState != null)
   //     {
   //         SwitchToTheNextState(nextState);
   //         if(nextState.GetType().Equals(typeof(IdleState)))
   //         {
   //             statusimage.color = Color.white;
   //         }
   //         if (nextState.GetType().Equals(typeof(ChaseState)))
   //         {
   //             statusimage.color = Color.yellow;
   //         }
   //         if (nextState.GetType().Equals(typeof(AttackState)))
   //         {
   //             statusimage.color = Color.red;
   //         }
   //     }
   // }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            bulletDetected = true;
           // detectedBullets.Add(collision.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            bulletDetected = false;
           // detectedBullets.Remove(collision.transform);
        }

    }
    private void SwitchToTheNextState(State nextState)
    {
        currentState = nextState;
    }
}
