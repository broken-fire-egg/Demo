using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem instance;
    public InventoryTetris playerInv;
    public InventoryTetris craftingInv;
    public InventoryTetris resultInv;
    public ItemRecipe[] recipes;
    private void Awake()
    {
        instance = this;
    }
    public void ClearMaterials()
    {
        for (int i = 0; i < craftingInv.itemContainer.childCount; i++)
        {
            var po = craftingInv.itemContainer.GetChild(i).GetComponent<PlacedObject>();
            craftingInv.RemoveItemAt(po.GetGridPosition());
        }
    }

    private void Clear()
    {
        PlacedObject po;
        for (int i = 0; i < craftingInv.itemContainer.childCount; i++)
        {
            po = craftingInv.itemContainer.GetChild(i).GetComponent<PlacedObject>();
            if (!po.Ghost)
            {
                playerInv.TryForcePlaceItem(po.GetPlacedObjectTypeSO() as ItemTetrisSO);
            }
                craftingInv.RemoveItemAt(po.GetGridPosition());
        }
        for (int i = 0; i < resultInv.itemContainer.childCount; i++)
        {
            po = resultInv.itemContainer.GetChild(i).GetComponent<PlacedObject>();
            resultInv.RemoveItemAt(po.GetGridPosition());
        }
    }
    public void ClearGhost()
    {
        PlacedObject po;
        for (int i = 0; i < craftingInv.itemContainer.childCount; i++)
        {
            po = craftingInv.itemContainer.GetChild(i).GetComponent<PlacedObject>();
            if (po.Ghost)
            {
                craftingInv.RemoveItemAt(po.GetGridPosition());
            }
        }
        for (int i = 0; i < resultInv.itemContainer.childCount; i++)
        {
            po = resultInv.itemContainer.GetChild(i).GetComponent<PlacedObject>();
            resultInv.RemoveItemAt(po.GetGridPosition());
        }
    }


    List<GameObject> temp;
    public void TryPlaceMaterialsByRecipe(ItemRecipe recipe)
    {
        //Remove Items in Craft & Result
        bool success = true;
        Clear();

        temp = new List<GameObject>();
        var inventory = playerInv.itemContainer;
        PlacedObject po;


        foreach (var material in recipe.materials)
        {
            bool allout = false;
            for (int i = 0; i < material.count; i++)
            {

                if (!allout)
                {
                    for (int j = 0; j < inventory.childCount; j++)
                    {
                        if (inventory.GetChild(j).name == material.material.nameString)
                        {
                            po = inventory.GetChild(j).GetComponent<PlacedObject>();
                            if (!temp.Contains(po.gameObject))
                            {
                                temp.Add(po.gameObject);
                                break;
                            }
                        }
                        if (j == inventory.childCount - 1)
                        {
                            allout = true;
                            Debug.Log("AllOut!");
                            break;
                        }
                    }
                }

                if (allout || inventory.childCount == 0)
                {
                    success = false;

                    playerInv.TryForcePlaceItem(material.material, out PlacedObject ghostPO, default, true);
                    temp.Add(ghostPO.gameObject);

                    //꽉찰때 예외처리 필요할듯?
                }
            }
        }

        foreach (var item in temp)
        {
            po = item.GetComponent<PlacedObject>();
            playerInv.RemoveItemAt(po.GetGridPosition());
            craftingInv.TryForcePlaceItem(po.GetPlacedObjectTypeSO() as ItemTetrisSO, default, po.Ghost);
        }



        foreach (var item in recipe.results)
        {
            for (int i = 0; i < item.count; i++)
            {
                resultInv.TryForcePlaceItem(item.material, default, !success);
            }
        }
    }

}

