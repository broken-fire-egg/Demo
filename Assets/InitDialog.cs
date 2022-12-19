using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class InitDialog : MonoBehaviour
{
    public string dialog_ID;
    public PlayableDirector pd;
    
    // Start is called before the first frame update
    void Start()
    {
        // DialogReader.instance.SetPage(dialog_ID);
        pd.gameObject.SetActive(false);
        DialogReader.instance.Read(dialog_ID);
        Camera.main.GetComponent<CameraMove>().enabled = true;
    }
    private void Update()
    {
        if(Input.anyKeyDown)
        {
           if(!DialogReader.instance.ReadNext())
                gameObject.SetActive(false);
        }
    }
}
