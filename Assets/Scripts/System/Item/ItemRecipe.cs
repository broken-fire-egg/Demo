using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;





[CreateAssetMenu()]
public class ItemRecipe : ScriptableObject
{
    [Serializable]
    public class ItemRecipeMaterial
    {
        public ItemTetrisSO material;
        public int count;
    }
    public ItemRecipeMaterial[] materials;
    public ItemRecipeMaterial[] results;




}
