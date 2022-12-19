using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using UnityEngine;

public class DialogReader : MonoBehaviour
{
    public static DialogReader instance;
    public string currentLine = "";

    public TextAsset textasset;
    string[] lines;
    int lineindex;
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
       // lineindex = 0;
        sr = new StreamReader(Application.dataPath + "/Resources/Dialog/Dialog.txt");
        //lines = textasset.text.SplitLines();
        print(sr.ReadLine());
    }

    string ReadLine()
    {
        return lines[lineindex];
    }

    public bool SetPage(string code)
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

            if (blocks[0].Equals(code))
            {
                break;
            }
        }
        return !eof;
    }

    public void Read(string code)
    {
        SetPage(code);
        print(currentLine);
        var info = currentLine.Split(',');
        DialogDisplayer.instance.SetPortrait(info);
        DialogDisplayer.instance.PlayDialogue(info[5]);
    }
    public void ReadString(string text)
    {
        var info = "objecttext, 0, None, None, 0, readingobjectstring".Split(',');
        DialogDisplayer.instance.SetPortrait(info);
        DialogDisplayer.instance.PlayDialogue(text);
    }
    public bool ReadNext()
    {
        var code = currentLine.Split(',')[0];
        currentLine = sr.ReadLine();
        var info = currentLine.Split(',');
        if (info[0] != null)
        {
            if (code.Equals(info[0]))
            {
                DialogDisplayer.instance.SetPortrait(info);
                DialogDisplayer.instance.PlayDialogue(info[5]);
                return true;
            }
        }
        DialogDisplayer.instance.CloseDialogue();
        return false;
    }
}
