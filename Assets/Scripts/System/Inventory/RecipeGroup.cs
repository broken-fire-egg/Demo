using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeGroup : MonoBehaviour
{
    public List<GameObject> recipes;
    private void Awake()
    {
        bool init = false;
        Transform c;
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            c = transform.parent.GetChild(i);
            if (!init)
                if (c.name == gameObject.name)
                {
                    init = true;
                    continue;
                }
                else
                    continue;




            if (!c.name.StartsWith("-----"))
                recipes.Add(c.gameObject);
            else
                break;
        }
    }

    public void SetVisible(bool condition)
    {
        foreach (var c in recipes)
        {
            c.SetActive(condition);
        }
    }
}
