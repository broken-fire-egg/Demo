using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ActiveAllDisplays : MonoBehaviour
{
    int width = 1730;
    int height = 570;
    int xpos = 130;
    int ypos = 240;

    int width2 = 1920;
    int height2 = 1000;
    int xpos2 = 0;
    int ypos2 = 0;

    public Text text;
    

    void Start()
    {
        Debug.Log("displays connected: " + Display.displays.Length);
        text.text = Display.displays.Length.ToString();
        if (Display.displays.Length >= 1)
        {
            Screen.fullScreen = false;
            Display.displays[0].SetParams(width2, height2, xpos2, ypos2);
            Display.displays[0].Activate(width2, height2, 60);

        }
        if (Display.displays.Length >= 2)
        {
            Screen.fullScreen = false;
            Display.displays[1].SetParams(width, height, xpos, ypos);
            Display.displays[1].Activate(width, height, 60);
        }
    }

    void Update()
    {

    }
}
