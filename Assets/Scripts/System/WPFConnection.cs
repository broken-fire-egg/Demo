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

public class WPFConnection : MonoBehaviour
{
    NamedPipeServerStream namedPipeServerStream;
    StreamString streamString;
    bool connection = false;
    public GameObject bombMsg;
    public Text bombPosText;
    int status = 0;
    Vector2 bombPos = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        Thread serverReadThread = new Thread(ServerThread_Read);
        serverReadThread.Start();
        Process.Start("TouchBomb.exe");
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
                bombMsg.SetActive(true);
                status = 0;
                break;
            case 2:
                bombMsg.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
                StartCoroutine(WindowPositionSetter.instance.Shake(2f,75, bombPos));
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
        namedPipeServerStream.Close();

    }

    public class StreamString
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;
        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }
        public string ReadString()
        {
            int len = 0;

            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            if (len > 0)
            {
                byte[] inBuffer = new byte[len];
                ioStream.Read(inBuffer, 0, len);
                return streamEncoding.GetString(inBuffer);
            }
            else
                return null;
        }
        public int WriteString(string outString)
        {
            byte[] outBuffer = streamEncoding.GetBytes(outString);
            int len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int)UInt16.MaxValue;
            }
            ioStream.WriteByte((byte)(len / 256));
            ioStream.WriteByte((byte)(len & 255));
            ioStream.Write(outBuffer, 0, len);
            ioStream.Flush();

            return outBuffer.Length + 2;
        }
    }

}
