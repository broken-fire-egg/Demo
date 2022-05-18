using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    class Alpha
    {
        public virtual void method(int level = 0) { Debug.Log("Alpha"); }
    }



    class Beta : Alpha

    {

        public override void method(int level = 0)
        {
            if (level > 0)
                base.method(level - 1);
            else
                Debug.Log("Beta"); 
        }

    }



    class Gamma : Beta

    {

        public override void method(int level = 0)
        {
            if (level > 0)
                base.method(level - 1);
            else
                Debug.Log("Gamma");
        }

    }


    public InventoryTetris inventory;
    private void Start()
    {

        Gamma g = new Gamma();
        g.method(0);
        g.method(1);
        g.method(2);
        //inventory.TryPlaceItem(InventoryTetrisAssets.Instance.ammo, new Vector2Int(0, 0), PlacedObjectTypeSO.Dir.Down);
        //inventory.TryPlaceItem(InventoryTetrisAssets.Instance.ammo, new Vector2Int(0, 1), PlacedObjectTypeSO.Dir.Down);
        //inventory.TryPlaceItem(InventoryTetrisAssets.Instance.shotgun, new Vector2Int(5, 5), PlacedObjectTypeSO.Dir.Down);
    }

}
