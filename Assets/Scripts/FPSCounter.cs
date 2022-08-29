using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    UnityEngine.UI.Text text;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SetFPS60", 1.5f);
       text = GetComponent<UnityEngine.UI.Text>();
    }
    public void SetFPS60()
    {
        Application.targetFrameRate = 60;
    }
    // Update is called once per frame
    void Update()
    {
        text.text = "FPS : " + (int)(1 / Time.unscaledDeltaTime);
    }
}
