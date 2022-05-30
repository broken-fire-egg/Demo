using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManagerTest : MonoBehaviour
{
    public bool bulletDetected = false;
    public State currentState;

    public GameObject IdleGame;
    public GameObject CautionGame;
    public GameObject AttackGame;
    public GameObject Colbullet;

    public Rigidbody2D rigid;

    public StateKind NowState;

    public bool IfChange = false;


    public enum StateKind
    {
        idle = 1,
        caution,
        Attack
    }

    void Awake()
    {
        NowState = StateKind.idle;
    }
    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.transform.parent.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Statefsm();
        StateCheck();
    }

    void Statefsm()
    {
        switch(NowState)
        {
            case StateKind.idle:
                IdleGame.SetActive(true);
                CautionGame.SetActive(false); AttackGame.SetActive(false);
                break;
            case StateKind.caution:
                CautionGame.SetActive(true);
                IdleGame.SetActive(false); AttackGame.SetActive(false);
                break;

            case StateKind.Attack:
                AttackGame.SetActive(true);
                CautionGame.SetActive(false); IdleGame.SetActive(false);
                break;

        }
    }

    void StateCheck()
    {
        State nextState = currentState?.RunCurrentState();
        SwitchToTheNextState(nextState);
        if (Vector2.Distance(transform.position, Hero.instance.transform.position) < 3.0f)
        {
            NowState = StateKind.Attack;
            gameObject.transform.GetChild(2).GetChild(0).GetComponent<ChaseState>().cooltime = 0;
        }
        
    }

    void AttackRelation()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            NowState = StateKind.Attack;
            bulletDetected = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            bulletDetected = false;
        }

    }
    private void SwitchToTheNextState(State nextState)
    {
        currentState = nextState;
    }
}
