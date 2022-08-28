using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    UnityEngine.UI.Text text;
    // Start is called before the first frame update
    void Start()
    {
       Application.targetFrameRate = 60;
       text = GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "FPS : " + (int)(1 / Time.unscaledDeltaTime);
    }
}
