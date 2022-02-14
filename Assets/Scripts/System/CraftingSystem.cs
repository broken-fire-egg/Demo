using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    
    public InventoryTetris playerInv;
    public InventoryTetris craftingInv;
    public InventoryTetris resultInv;
    public ItemRecipe[] recipes;

    public void CheckResult()
    {
       for(int i=0 ; i < craftingInv.GetItemContainer().childCount;i++)
        {
            Transform item = craftingInv.GetItemContainer().GetChild(i);
            
        }
    }


}

