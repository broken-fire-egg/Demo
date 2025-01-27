﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTetris : MonoBehaviour
{

    public static InventoryTetris Instance { get; private set; }
    public InventoryTetrisBackground InventoryBackground;
    public event EventHandler<PlacedObject> OnObjectPlaced;
    public int gridWidth;
    public int gridHeight;
    public bool onlyTake;
    public float cellSize = 90f;
    private Grid<GridObject> grid;
    [HideInInspector] public RectTransform itemContainer;


    private void Awake()
    {
        Instance = this;



        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));

        itemContainer = transform.Find("ItemContainer").GetComponent<RectTransform>();

        transform.Find("BackgroundTempVisual").gameObject.SetActive(false);
    }

    public class GridObject
    {

        private Grid<GridObject> grid;
        private int x;
        private int y;
        public PlacedObject placedObject;

        public GridObject(Grid<GridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            placedObject = null;
        }

        public override string ToString()
        {
            return x + ", " + y + "\n" + placedObject;
        }

        public void SetPlacedObject(PlacedObject placedObject)
        {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void ClearPlacedObject()
        {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, y);
        }

        public PlacedObject GetPlacedObject()
        {
            return placedObject;
        }

        public bool CanBuild()
        {
            return placedObject == null;
        }

        public bool HasPlacedObject()
        {
            return placedObject != null;
        }

    }

    public Grid<GridObject> GetGrid()
    {
        return grid;
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        grid.GetXY(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public bool IsValidGridPosition(Vector2Int gridPosition)
    {
        return grid.IsValidGridPosition(gridPosition);
    }
    public bool CheckCanPlaceItem(ItemTetrisSO itemTetrisSO, Vector2Int placedObjectOrigin, PlacedObjectTypeSO.Dir dir, bool isUserMove = true)
    {
        if (dir == PlacedObjectTypeSO.Dir.None)
            dir = itemTetrisSO.defaultDir;
        List<Vector2Int> gridPositionList = itemTetrisSO.GetGridPositionList(placedObjectOrigin, dir);
        bool canPlace = true;
        if (onlyTake && isUserMove)
            canPlace = false;
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            bool isValidPosition = grid.IsValidGridPosition(gridPosition);
            if (!isValidPosition)
            {
                // Not valid
                canPlace = false;
                break;
            }
            if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
            {
                canPlace = false;
                break;
            }
        }

        if (canPlace)
        {
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canPlace = false;
                    break;
                }
            }
        }

        foreach (var xy in gridPositionList)
        {
            if (xy.x < GetGrid().GetWidth() && xy.x >= 0 && xy.y < GetGrid().GetHeight() && xy.y >= 0)
                try
                {
                    InventoryBackground.backgrounds[xy.x, xy.y].color = Color.gray;
                }
                catch
                {
                    Debug.Log(xy.x.ToString() + "/" + xy.y.ToString());
                }
        }





        return canPlace;
    }
    public bool TryForcePlaceItem(ItemTetrisSO itemTetrisSO,out PlacedObject po, PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.None, bool ghostMode = false)
    {
        if (dir == PlacedObjectTypeSO.Dir.None)
            dir = itemTetrisSO.defaultDir;
        int x = 0, y = gridHeight - 1;
        bool res = false;
        Vector2Int tempos;
        po = null;
        for (int i = 0; i < 4; i++)
        {
            while (!res)
            {
                tempos = new Vector2Int(x, y);
                if (grid.IsValidGridPosition(tempos))
                {
                    res = TryPlaceItem(itemTetrisSO, tempos, dir,out po, false, ghostMode);

                }

                x++;
                if (x >= gridWidth)
                {
                    x = 0;
                    y--;
                }
                if (y < 0)
                    break;
            }
            if (res)
                break;
            dir = PlacedObjectTypeSO.GetNextDir(dir);
        }
        Debug.Log(res);
        return res;
    }
    public bool TryForcePlaceItem(ItemTetrisSO itemTetrisSO, PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.None, bool ghostMode = false)
    {
        if (dir == PlacedObjectTypeSO.Dir.None)
            dir = itemTetrisSO.defaultDir;
        int x = 0, y = gridHeight - 1;
        bool res = false;
        Vector2Int tempos;
        for (int i = 0; i < 4; i++)
        {
            while (!res)
            {
                tempos = new Vector2Int(x, y);
                if (grid.IsValidGridPosition(tempos))
                {
                    res = TryPlaceItem(itemTetrisSO, tempos, dir, false, ghostMode);

                }

                x++;
                if (x >= gridWidth)
                {
                    x = 0;
                    y--;
                }
                if (y < 0)
                    break;
            }
            if (res)
                break;
            dir = PlacedObjectTypeSO.GetNextDir(dir);
        }
        Debug.Log(res);
        return res;
    }

    public bool TryPlaceItem(ItemTetrisSO itemTetrisSO, Vector2Int placedObjectOrigin, PlacedObjectTypeSO.Dir dir, out PlacedObject po, bool isUserMove = true, bool ghostMode = false)
    {
        // Test Can Build
        if (dir == PlacedObjectTypeSO.Dir.None)
            dir = itemTetrisSO.defaultDir;
        List<Vector2Int> gridPositionList = itemTetrisSO.GetGridPositionList(placedObjectOrigin, dir);
        bool canPlace = true;
        if (onlyTake && isUserMove)
            canPlace = false;
        po = null;
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            bool isValidPosition = grid.IsValidGridPosition(gridPosition);
            if (!isValidPosition)
            {
                // Not valid
                canPlace = false;
                break;
            }
            if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
            {
                canPlace = false;
                break;
            }
        }

        if (canPlace)
        {
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canPlace = false;
                    break;
                }
            }
        }

        if (canPlace)
        {
            Vector2Int rotationOffset = itemTetrisSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();

            PlacedObject placedObject = PlacedObject.CreateCanvas(itemContainer, placedObjectWorldPosition, placedObjectOrigin, dir, itemTetrisSO);
            placedObject.transform.rotation = Quaternion.Euler(0, 0, -itemTetrisSO.GetRotationAngle(dir));
            if (ghostMode)
                placedObject.Ghost = true;
            placedObject.GetComponent<InventoryTetrisDragDrop>().Setup(this);

            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
            }

            OnObjectPlaced?.Invoke(this, placedObject);
            po = placedObject;
            // Object Placed!
            return true;
        }
        else
        {
            // Object CANNOT be placed!
            return false;
        }
    }

    public bool TryPlaceItem(ItemTetrisSO itemTetrisSO, Vector2Int placedObjectOrigin, PlacedObjectTypeSO.Dir dir, bool isUserMove = true, bool ghostMode = false)
    {
        // Test Can Build
        if (dir == PlacedObjectTypeSO.Dir.None)
            dir = itemTetrisSO.defaultDir;
        List<Vector2Int> gridPositionList = itemTetrisSO.GetGridPositionList(placedObjectOrigin, dir);
        bool canPlace = true;
        if (onlyTake && isUserMove)
            canPlace = false;
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            bool isValidPosition = grid.IsValidGridPosition(gridPosition);
            if (!isValidPosition)
            {
                // Not valid
                canPlace = false;
                break;
            }
            if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
            {
                canPlace = false;
                break;
            }
        }

        if (canPlace)
        {
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canPlace = false;
                    break;
                }
            }
        }

        if (canPlace)
        {
            Vector2Int rotationOffset = itemTetrisSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();

            PlacedObject placedObject = PlacedObject.CreateCanvas(itemContainer, placedObjectWorldPosition, placedObjectOrigin, dir, itemTetrisSO);
            placedObject.transform.rotation = Quaternion.Euler(0, 0, -itemTetrisSO.GetRotationAngle(dir));
            if (ghostMode)
                placedObject.Ghost = true;
            placedObject.GetComponent<InventoryTetrisDragDrop>().Setup(this);

            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
            }

            OnObjectPlaced?.Invoke(this, placedObject);

            // Object Placed!
            return true;
        }
        else
        {
            // Object CANNOT be placed!
            return false;
        }
    }

    public void TemporaryClear(PlacedObject PlacedObject)
    {

        if (PlacedObject != null)
        {
            List<Vector2Int> gridPositionList = PlacedObject.GetGridPositionList();
            foreach (Vector2Int gridPosition in gridPositionList)
                grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
        }
    }


    public void RemoveItemAt(Vector2Int removeGridPosition)
    {
        PlacedObject placedObject = grid.GetGridObject(removeGridPosition.x, removeGridPosition.y).GetPlacedObject();

        if (placedObject != null)
        {
            // Demolish
            placedObject.DestroySelf();
            List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
            }
        }
    }

    public RectTransform GetItemContainer()
    {
        return itemContainer;
    }



    [Serializable]
    public struct AddItemTetris
    {
        public string itemTetrisSOName;
        public Vector2Int gridPosition;
        public PlacedObjectTypeSO.Dir dir;
    }

    [Serializable]
    public struct ListAddItemTetris
    {
        public List<AddItemTetris> addItemTetrisList;
    }

    public string Save()
    {
        List<PlacedObject> placedObjectList = new List<PlacedObject>();
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                if (grid.GetGridObject(x, y).HasPlacedObject())
                {
                    placedObjectList.Remove(grid.GetGridObject(x, y).GetPlacedObject());
                    placedObjectList.Add(grid.GetGridObject(x, y).GetPlacedObject());
                }
            }
        }

        List<AddItemTetris> addItemTetrisList = new List<AddItemTetris>();
        foreach (PlacedObject placedObject in placedObjectList)
        {
            addItemTetrisList.Add(new AddItemTetris
            {
                dir = placedObject.GetDir(),
                gridPosition = placedObject.GetGridPosition(),
                itemTetrisSOName = (placedObject.GetPlacedObjectTypeSO() as ItemTetrisSO).name,
            });

        }

        return JsonUtility.ToJson(new ListAddItemTetris { addItemTetrisList = addItemTetrisList });
    }

    public void Load(string loadString)
    {
        ListAddItemTetris listAddItemTetris = JsonUtility.FromJson<ListAddItemTetris>(loadString);

        foreach (AddItemTetris addItemTetris in listAddItemTetris.addItemTetrisList)
        {
            TryPlaceItem(InventoryTetrisAssets.Instance.GetItemTetrisSOFromName(addItemTetris.itemTetrisSOName), addItemTetris.gridPosition, addItemTetris.dir);
        }
    }

}
