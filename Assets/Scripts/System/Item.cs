using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public enum ItemType { 
        Gun,
        Bullet,
        Module,
        Material,
        Consumables,
    }
    public ItemType itemType;
    public int amount;


}
