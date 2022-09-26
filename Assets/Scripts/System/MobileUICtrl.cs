using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MobileUICtrl : MonoBehaviour
{
    public GameObject BlackSprite;
    public GameObject PhoneSprite;
    Transform _PhoneSpritetf;
    Sequence seq;

    public bool Phonebool;                  //핸드폰 활성화확인
    public State PhoneState;
    public bool AppActivate = false;
    bool PhoneAnimation = false;
    private bool PhoneBool = false;
    public List<GameObject> phone_Screen_UIs;
    public enum State
    {
        None = 0,
        Bag = 1,
        Skill = 2,
        Map = 3,
        Alarm = 4,
        Call = 5,
        Camera = 6,
        Craft = 7
    }

    
    public List<GameObject> UiButton = new List<GameObject>();
    private void Awake()
    {
        _PhoneSpritetf = PhoneSprite.GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {

        
        //MapGame.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(MapGame.transform.GetChild(1).gameObject.GetComponent<Image>().color.r, MapGame.transform.GetChild(1).gameObject.GetComponent<Image>().color.g, MapGame.transform.GetChild(1).gameObject.GetComponent<Image>().color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (PhoneBool == false)
            {
                _PhoneSpritetf.DOMoveY(300, 1.0f).SetEase(Ease.OutBounce);
                PhoneBool = true;
            }
            else
            {
                if (PhoneState == State.None)
                {
                    _PhoneSpritetf.DOMoveY(-865, 1.0f).SetEase(Ease.Flash);
                    PhoneBool = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (AppActivate == true && PhoneAnimation == true)
            {
                PhoneRemove();
                PhoneAnimation = false;
            }
            else if (AppActivate == true && PhoneAnimation == false)
            {
                AllButtonOn();
                foreach(var ui in phone_Screen_UIs)
                {
                    ui.SetActive(false);
                }
            }
            PhoneState = State.None;
        }
    }
    public void InvokeAppearObj(GameObject obj)
    {
        StartCoroutine(InvokeAppearObjCor(obj));
    }
    WaitForSeconds ws = new WaitForSeconds(0.5f);
    IEnumerator InvokeAppearObjCor(GameObject obj)
    {
        yield return ws;
        obj.SetActive(true);
    }

    public void testButton()
    {
        Debug.Log("a");
        AppActivate = true;
    }

    public void BagButton()
    {
        PhoneState = State.Bag;
        AllButtonOff(0, true);
        PhoneAnimation = true;
    }

    public void SkillButton()
    {
        PhoneState = State.Skill;
        AllButtonOff(1, true);
        PhoneAnimation = true;
    }
    public void MapButton()
    {
        PhoneState = State.Map;
        AllButtonOff(2, true);
        seq = DOTween.Sequence();
        seq.AppendInterval(2.0f);
        seq.AppendCallback(() => { UiButton[2].transform.GetChild(1).gameObject.GetComponent<Image>().gameObject.SetActive(true); });
        PhoneAnimation = true;
    }

    public void AlarmButton()
    {
        PhoneState = State.Alarm;
        AllButtonOff(3, false);

        UiButton[3].transform.GetChild(1).gameObject.SetActive(true); 
    }


    public void CallButton()
    {
        PhoneState = State.Call;
        AllButtonOff(4, false);

        UiButton[4].transform.GetChild(0).gameObject.SetActive(false);
        UiButton[4].transform.GetChild(1).gameObject.SetActive(true);
    }

    public void CameraButton()
    {
        PhoneState = State.Camera;
        AllButtonOff(5, true);
        // UiButton[5].transform.GetChild(1).gameObject.SetActive(false);
        PhoneAnimation = true;
    }

    public void CraftButton()
    {
        PhoneState = State.Craft;
        AllButtonOff(6, true);
        PhoneAnimation = true;

    }
    public void aFewMomentLater()
    {
        seq = DOTween.Sequence();
        BlackSprite.SetActive(true);
        BlackSprite.GetComponent<Image>().DOFade(1, 1.0f);
        BlackSprite.GetComponent<Image>().DOFade(0, 1.0f).SetDelay(1.5f);
        seq.AppendInterval(2.5f);
        seq.AppendCallback(() => BlackSprite.SetActive(false));
    }

    void delete()
    {
        //UiButton[5].transform.GetChild(1).gameObject.SetActive(false);
    }

    void ObjectTransformChange(GameObject ForObjectGame)
    {
        ForObjectGame.GetComponent<RectTransform>().DOMove(new Vector2(960, 540), 1f);
        ForObjectGame.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 90), 1f);
        ForObjectGame.GetComponent<RectTransform>().DOScale(new Vector3(10, 20, 10), 1f);
    }
   
    void PhoneMove()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        PhoneSprite.SetActive(true);
        ObjectTransformChange(PhoneSprite);
        ObjectTransformChange(BlackSprite);
        BlackSprite.SetActive(true);
        BlackSprite.GetComponent<Image>().DOFade(1, 1.0f);
        BlackSprite.GetComponent<Image>().DOFade(0, 1.0f).SetDelay(1.0f);
    }

    void PhoneRemove()
    {
        seq = DOTween.Sequence();
        _PhoneSpritetf.DOScale(new Vector3(1, 2, 1), 1f);
        _PhoneSpritetf.DOMove(gameObject.transform.GetChild(0).transform.position, 1f);
        _PhoneSpritetf.DORotate(new Vector3(0, 0, 0), 1f);
        BlackSprite.GetComponent<Image>().DOFade(0, 1.0f);

        seq.AppendInterval(1f);
        seq.AppendCallback(() => { PhoneSprite.SetActive(false); });
        seq.AppendCallback(AllButtonOn);
        seq.AppendCallback(() => { BlackSprite.SetActive(false); });
    }
    void AllButtonOff(int ButtonNum, bool Move)
    {
        for (int i = 0; i < 7; i++)
            UiButton[i].SetActive(false);
        UiButton[ButtonNum].SetActive(true);
        UiButton[ButtonNum].transform.GetChild(0).gameObject.SetActive(false);
        AppActivate = true;
        if(Move == true)
            PhoneMove();
    }

    void AllButtonOn()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < UiButton[i].transform.childCount; j++)
                UiButton[i].transform.GetChild(j).gameObject.SetActive(false);
            UiButton[i].SetActive(true);
            UiButton[i].transform.GetChild(0).gameObject.SetActive(true);
            UiButton[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(UiButton[i].transform.GetChild(0).gameObject.GetComponent<Image>().color.r, UiButton[i].transform.GetChild(0).gameObject.GetComponent<Image>().color.g, UiButton[i].transform.GetChild(0).gameObject.GetComponent<Image>().color.b, 1);
        }
        AppActivate = false;
    }
}
