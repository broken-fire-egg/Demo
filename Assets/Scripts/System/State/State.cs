using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{

    public abstract State RunCurrentState();
    public StateManager sManager;
    public State[] nextState;

    // Start is called before the first frame update
    void Start()
    {
        sManager = transform.parent.GetComponent<StateManager>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
