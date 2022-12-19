using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogDisplayer : MonoBehaviour
{
    static public DialogDisplayer instance;

    void Awake()
    {
        instance = this;
        dialogueVertexAnimator = new DialogueVertexAnimator(textBox, audioSourceGroup);
    }
    private void Start()
    {
        StartCoroutine(PortraitCoroutine());

    }
    public GameObject dialogueWindow;
    public bool displaying;
    public TMP_Text textBox;
    public TMP_Text nameBox;
    public AudioClip typingClip;
    public AudioSourceGroup audioSourceGroup;
    public Image leftPortrait;
    public Image rightPortrait;
    public DataMappingSO mappedData;
    bool lpFocus;
    bool rpFocus;

    public void SetPortrait(string[] info)
    {
        SetPortaritFocus(int.Parse(info[4]));
        SetPortraitSprite(info);
    }
    void SetPortraitSprite(string[] info)
    {
        Debug.Log(info[2] + info[3]);
        foreach (var md in mappedData.portraits)
        {
            if (md.key.Equals(info[2].Trim()))
            {
                leftPortrait.sprite = md.value;
                leftPortrait.SetNativeSize();
                break;
            }
        }
        foreach (var md in mappedData.portraits)
        {
            if (md.key.Equals(info[3].Trim()))
            {
                rightPortrait.sprite = md.value;

                rightPortrait.SetNativeSize();
                break;
            }
        }
    }
    void SetPortaritFocus(int n)
    {
        switch(n)
        {
            case 0:
                nameBox.text = "";
                lpFocus = false;
                rpFocus = false;
                break;
            case 1:
                nameBox.text = DialogReader.instance.currentLine.Split(',')[2];
                lpFocus = true;
                rpFocus = false;
                break;
            case 2:
                print(DialogReader.instance.currentLine);
                nameBox.text = DialogReader.instance.currentLine.Split(',')[3];
                lpFocus = false;
                rpFocus = true;
                break;
            case 3:
                nameBox.text = "All";
                lpFocus = true;
                rpFocus = true;
                break;
            default:
                break;
        }
    }

    IEnumerator PortraitCoroutine()
    {
        float speed = 0.005f;
        while(true)
        {
            if(lpFocus)
            {
                if (leftPortrait.color.r < 1f)
                    leftPortrait.color = new Color(leftPortrait.color.r + speed, leftPortrait.color.r + speed, leftPortrait.color.r + speed);
            }
            else
            {
                if (leftPortrait.color.r > 0.5f)
                    leftPortrait.color = new Color(leftPortrait.color.r - speed, leftPortrait.color.r - speed, leftPortrait.color.r - speed);
            }
            if (rpFocus)
            {
                if (rightPortrait.color.r < 1f)
                    rightPortrait.color = new Color(rightPortrait.color.r + speed, rightPortrait.color.r + speed, rightPortrait.color.r + speed);
            }
            else
            {
                if (rightPortrait.color.r > 0.5f)
                    rightPortrait.color = new Color(rightPortrait.color.r - speed, rightPortrait.color.r - speed, rightPortrait.color.r - speed);
            }
            yield return null;
        }
    }

    private DialogueVertexAnimator dialogueVertexAnimator;


    private void PlayDialogue1()
    {
        //PlayDialogue(dialogue1);
    }

    private Coroutine typeRoutine = null;


    public void OpenDialogue()
    {
        displaying = true;
        dialogueWindow.SetActive(true);
    }
    public void CloseDialogue()
    {
        displaying = false;
        dialogueWindow.SetActive(false);
    }
    public void PlayDialogue(string message)
    {
        OpenDialogue();
        this.EnsureCoroutineStopped(ref typeRoutine);
        dialogueVertexAnimator.textAnimating = false;

        message = message.Replace('^', ',');


        List<DialogueCommand> commands = DialogueUtility.ProcessInputString(message, out string totalTextMessage);
        typeRoutine = StartCoroutine(dialogueVertexAnimator.AnimateTextIn(commands, totalTextMessage, typingClip, null));
    }
    public void EnsureCoroutineStopped(ref Coroutine routine)
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }
    }
}
