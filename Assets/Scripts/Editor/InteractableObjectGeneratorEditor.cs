using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractableObjectGenerator))]
public class InteractableObjectGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var generator = (InteractableObjectGenerator)target;
        if(generator.sprite)
        {
           GUI.DrawTexture(new Rect(10, 120, generator.sprite.rect.width, generator.sprite.rect.height), generator.sprite.texture, ScaleMode.StretchToFill, true, 10.0F);
        }
        EditorGUILayout.Space(generator.sprite.rect.height + 25);

        var guistyle = new GUIStyle(GUI.skin.button);
        guistyle.fontSize = 24;
        guistyle.fixedHeight = 50;

        if (GUILayout.Button("오브젝트 생성하기", guistyle))
        {
            generator.Generate();
        }
    }
}
