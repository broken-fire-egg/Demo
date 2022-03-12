using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTutorialArea : MonoBehaviour
{
    public float speed;
    public List<Collider2D> standingObjectColliders;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        foreach(var obj in standingObjectColliders)
        {
            obj.TryGetComponent<HeroMove>(out HeroMove hero);
            if(hero)
                if(hero.onground)
                    obj.transform.Translate(Vector2.down * speed);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        standingObjectColliders.Add(collision);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision)
            standingObjectColliders.Remove(collision);
    }

}
