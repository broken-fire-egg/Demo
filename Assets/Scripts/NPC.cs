using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    bool firsttalk = true;
    bool closed;
    bool triedToInteract;
    private void Update()
    {
        if (closed)
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (firsttalk)
                {
                    DialogReader.instance.Read(1);
                    firsttalk = false;
                }
                else
                    DialogReader.instance.ReadNext();
            }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        closed = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        closed = false;
    }
}
