using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseOverEvent : MonoBehaviour
{
    public UnityEvent EnterEventList;
    public UnityEvent ExitEventList;
    int UILayer;
    bool prev;
    private void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
    }
    private void Update()
    {
        if(prev != IsPointerOverUIElement())
        {
            prev = !prev;
            if (prev)
                EnterEventList.Invoke();
            else
                ExitEventList.Invoke();
        }
    }
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject == gameObject)
                return true;
        }
        return false;
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
