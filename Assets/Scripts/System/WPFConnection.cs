using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityController;
public class WPFConnection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityCommands.StartServer("50000");
    }

    // Update is called once per frame
    void Update()
    {
        UnityCommands.ReceiveMessage();
    }
}
