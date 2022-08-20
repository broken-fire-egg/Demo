using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSensor : MonoBehaviour
{
    Collider2D doorCollider;
    public SpriteRenderer doorSR;
    public Sprite[] sprites;
    float progress; //0 == fully closed, 1 == fully open
    public float speed;
    public bool verticalDoor;
    public bool condition;
    List<Collider2D> standingObjectColliders;
    // Start is called before the first frame update
    void Start()
    {
        doorCollider = transform.parent.GetComponent<Collider2D>();
        progress = 0;
        standingObjectColliders = new List<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (verticalDoor)
            if (transform.position.y < Hero.instance.transform.position.y)
            {
                doorSR.sortingOrder = 3;
            }
            else
            {
                doorSR.sortingOrder = 1;
            }

        if (!condition)
            return;


        if(standingObjectColliders.Count > 0)
        {
            progress += speed;
            if (progress > 1)
                progress = 1;
        }
        else
        {
            progress -= speed;
            if (progress < 0)
                progress = 0;
        }
        if (progress >= 1)
            doorCollider.enabled = false;
        else
            doorCollider.enabled = true;
        int i;
        for (i = 0;i< sprites.Length-1;i++)
        {
            if(progress <= i * (float)1/ (float)sprites.Length)
                break;

        }
        doorSR.sprite = sprites[i];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        standingObjectColliders.Add(collision);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision)
            standingObjectColliders.Remove(collision);
    }
}
