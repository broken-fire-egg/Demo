using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class StopBuiltInGame : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyUp(KeyCode.Space))
            EditorApplication.isPaused = true;
#endif
    }
}
