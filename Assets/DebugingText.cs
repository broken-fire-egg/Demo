using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugingText : MonoBehaviour
{

    static public DebugingText instance;
    public Text text; 
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }
}
