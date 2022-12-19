using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InteractableObjectGenerator : MonoBehaviour
{
    [SerializeField]
    public Sprite sprite;
    [SerializeField]
    [TextArea]
    string text;
    [SerializeField]
    public void Generate()
    {
        GameObject go = new GameObject(sprite.name,typeof(Interacable),typeof(BoxCollider2D),typeof(SpriteRenderer));
        go.GetComponent<Interacable>().text = text;
        go.GetComponent<SpriteRenderer>().sprite = sprite;
        var col = go.GetComponent<BoxCollider2D>();
        col.size = new Vector2(sprite.rect.width / 100f + 1, sprite.rect.height / 100f + 1);
        col.isTrigger = true;
        go.AddComponent<BoxCollider2D>();

        Selection.objects = new Object[] { go };
        
    }
}
