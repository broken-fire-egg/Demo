using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacable : MonoBehaviour
{
    Collider2D sensor;
    bool touching;
    public string textID;
    public string text;
    bool interacted;
    private void Awake()
    {
        sensor = GetComponent<Collider2D>();

    }
    private void Update()
    {
        if (!DialogDisplayer.instance.displaying)
            if (touching)
                if (Input.GetKeyDown(KeyCode.F))
                    Interact();

    }
    void Interact()
    {
        sensor.enabled = false;
        if (textID.Equals(""))
        {
            DialogReader.instance.ReadString(text);
        }
        else
            DialogReader.instance.Read(textID);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
            touching = true;
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
            touching = false;
        
    }
}
