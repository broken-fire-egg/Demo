using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogReader : MonoBehaviour
{
    public static DialogReader instance;
    public string currentLine = "";
    StreamReader sr;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Init();
    }

    void Init()
    {
        sr = new StreamReader(Application.dataPath + "/Resources/Dialog/Test.txt");
        print(sr.ReadLine());
    }

    public bool SetPage(int code)
    {
        Init();
        bool eof = false;
        while (!eof)
        {
            currentLine = sr.ReadLine();
            if (currentLine == null)
            {
                eof = true;
                break;
            }
            var blocks = currentLine.Split(',');

            if (int.Parse(blocks[0]) == code)
            {
                break;
            }
        }
        return !eof;
    }

    public void Read(int code)
    {
        SetPage(code);
        print(currentLine);
        DialogDisplayer.instance.PlayDialogue(currentLine.Split(',')[5]);
        DialogDisplayer.instance.SetPortaritFocus(int.Parse(currentLine.Split(',')[4]));
    }

    public bool ReadNext()
    {
        var code = currentLine.Split(',')[0];
        currentLine = sr.ReadLine();
        if (currentLine.Split(',')[0] != null)
        {
            if (code.Equals(currentLine.Split(',')[0]))
            {
                
                DialogDisplayer.instance.SetPortaritFocus(int.Parse(currentLine.Split(',')[4]));
                DialogDisplayer.instance.PlayDialogue(currentLine.Split(',')[5]);
                return true;
            }
        }
        DialogDisplayer.instance.CloseDialogue();
        return false;
    }
}
