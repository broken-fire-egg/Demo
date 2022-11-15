using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PlacedObject : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler
{
    bool isMouseOver;
    public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO)
    {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));
        placedObjectTransform.name = placedObjectTransform.name.Replace("(Clone)", "");
        PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();
        placedObject.placedObjectTypeSO = placedObjectTypeSO;
        placedObject.origin = origin;
        placedObject.dir = dir;

        placedObject.Setup();

        return placedObject;
    }

   
    public static PlacedObject CreateCanvas(Transform parent, Vector2 anchoredPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO)
    {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, parent);
        placedObjectTransform.name = placedObjectTransform.name.Replace("(Clone)", "");
        placedObjectTransform.rotation = Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
        placedObjectTransform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

        PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();
        placedObject.placedObjectTypeSO = placedObjectTypeSO;
        placedObject.origin = origin;
        placedObject.dir = dir;

        placedObject.Setup();

        return placedObject;
    }



    private PlacedObjectTypeSO placedObjectTypeSO;
    private Vector2Int origin;
    private PlacedObjectTypeSO.Dir dir;
    private UnityEngine.UI.Image poImage;
    private bool ghost;

    public bool Ghost
    {
        get => ghost; 
        set
        {
            ghost = value; 
            if (value) 
                poImage.color = new Color(1, 0.5f, 0.5f, 0.5f);
            else
                poImage.color = Color.white;
        }
    }

    private void Awake()
    {
        poImage = GetComponentInChildren<UnityEngine.UI.Image>();
    }
    protected virtual void Setup()
    {
        //Debug.Log("PlacedObject.Setup() " + transform);
    }

    public virtual void GridSetupDone()
    {
        //Debug.Log("PlacedObject.GridSetupDone() " + transform);
    }

    protected virtual void TriggerGridObjectChanged()
    {
        foreach (Vector2Int gridPosition in GetGridPositionList())
        {
            GridBuildingSystem3D.Instance.GetGridObject(gridPosition).TriggerGridObjectChanged();
        }
    }

    public Vector2Int GetGridPosition()
    {
        return origin;
    }

    public void SetOrigin(Vector2Int origin)
    {
        this.origin = origin;
    }

    public List<Vector2Int> GetGridPositionList()
    {
        return placedObjectTypeSO.GetGridPositionList(origin, dir);
    }

    public PlacedObjectTypeSO.Dir GetDir()
    {
        return dir;
    }

    public virtual void DestroySelf()
    {
        Destroy(gameObject);
    }

    public override string ToString()
    {
        return placedObjectTypeSO.nameString;
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO()
    {
        return placedObjectTypeSO;
    }



    public SaveObject GetSaveObject()
    {
        return new SaveObject
        {
            placedObjectTypeSOName = placedObjectTypeSO.name,
            origin = origin,
            dir = dir,
            //floorPlacedObjectSave = (this is FloorPlacedObject) ? ((FloorPlacedObject)this).Save() : "",
        };
    }

    string GenerateTooltipText()
    {
        string res = "<align=\"center\"><b>";
        switch (placedObjectTypeSO.rarity)
        {
            case PlacedObjectTypeSO.Rarity.Uncommon:
                res += "<color=green>";
                break;
            case PlacedObjectTypeSO.Rarity.Rare:
                res += "<color=blue>";
                break;
            case PlacedObjectTypeSO.Rarity.Unique:
                res += "<color=purple>";
                break;
            case PlacedObjectTypeSO.Rarity.Legend:
                res += "<color=orange>";
                break;
            default:
                res += "<color=white>";
                break;
        }

        res += placedObjectTypeSO.nameString + "</color></b></align>\n\n<align=\"left\">";

        if(!placedObjectTypeSO.effectText.Equals(""))
            res += placedObjectTypeSO.effectText + "\n\n";

        res += placedObjectTypeSO.description;

        return res;
    }

    private void Update()
    {
        if (isMouseOver)
            if (Input.GetMouseButtonDown(1))
                UseItem();
    }
    void UseItem()
    {
        ConsumableItemEventLibrary.PlayEvent(placedObjectTypeSO.nameString);
        ToolTip.instance.HideToolTip();
        DestroySelf();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        ToolTip.instance.ShowToolTip(GenerateTooltipText());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        Debug.Log(placedObjectTypeSO.nameString + " Exit");
        ToolTip.instance.HideToolTip();
    }

    [System.Serializable]
    public class SaveObject
    {

        public string placedObjectTypeSOName;
        public Vector2Int origin;
        public PlacedObjectTypeSO.Dir dir;
        public string floorPlacedObjectSave;

    }

}
