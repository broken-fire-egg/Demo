using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public InventoryTetris inventory;
    private void Start()
    {
        inventory.TryPlaceItem(InventoryTetrisAssets.Instance.ammo, new Vector2Int(0, 0), PlacedObjectTypeSO.Dir.Down);

        inventory.TryPlaceItem(InventoryTetrisAssets.Instance.shotgun, new Vector2Int(5, 5), PlacedObjectTypeSO.Dir.Down);
    }

}
