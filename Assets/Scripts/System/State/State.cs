using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{

    public abstract State RunCurrentState();
    public StateManager sManager;
    public StateManagerTest sManagerTest;
    public State[] nextState;

    // Start is called before the first frame update
    void Awake()
    {
        sManager = transform.parent.parent.GetComponent<StateManager>();
        sManagerTest = transform.parent.parent.GetComponent<StateManagerTest>();

    }
}