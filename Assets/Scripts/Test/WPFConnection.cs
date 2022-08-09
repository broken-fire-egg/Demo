using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityController;
using System.IO;
using System.Text;
using System.IO.Pipes;
using System.Threading;
using System;
using System.Diagnostics;
using UnityEngine.UI;
using System.Runtime.InteropServices;
public class WPFConnection : MonoBehaviour
{
    NamedPipeServerStream namedPipeServerStream;
    StreamString streamString;
    bool connection = false;
    public GameObject bombMsg;
    public Text bombPosText;
    int status = 0;
    Vector2 bombPos = Vector2.zero;




    [DllImport("user32.dll")]

    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);



    [DllImport("user32.dll")]

    private static extern IntPtr GetActiveWindow();


    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        //Thread serverReadThread = new Thread(ServerThread_Read);
        //serverReadThread.Start();
        Process.Start("WinSeparator.exe");
        StartCoroutine(delay());
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(1f);
        ShowWindow(GetActiveWindow(), 6);

    }
    void ServerThread_Read()
    {
        namedPipeServerStream = new NamedPipeServerStream("BombPipe", PipeDirection.In);
        namedPipeServerStream.WaitForConnection();
        UnityEngine.Debug.Log("Client Detected");
        connection = true;
        streamString = new StreamString(namedPipeServerStream);

        while (connection)
        {
            string message = streamString.ReadString();
            UnityEngine.Debug.Log("Recived: " + message);
            if (message != null)
            {
                if (message.Contains("Bomb Planted!"))
                {
                    UnityEngine.Debug.Log(message);

                    message = message.Replace("Bomb Planted!", "");

                    bombPosText.text = message;
                    bombPos = new Vector2(int.Parse(message.Split(',')[0]), int.Parse(message.Split(',')[1]));
                    DebugingText.instance.text.text = message.Split(',')[0] + "," + message.Split(',')[1];
                    status = 1;
                }
                if (message == "Bomb Exploded!")
                {
                    status = 2;
                    connection = false;
                }
            }
        }
    }
    private void Update()
    {
        switch (status)
        {
            case 1:
                status = 0;
                break;
            case 2:
                bombMsg.SetActive(true);
                bombMsg.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
                StartCoroutine(WindowPositionSetter.instance.Shake(4f,300, bombPos));
                status = 0;
                break;
            default:
                break;
        }
    }


    private void FixedUpdate()
    {

    }
    private void OnApplicationQuit()
    {
        if (namedPipeServerStream != null)
            namedPipeServerStream.Close();

    }

}
