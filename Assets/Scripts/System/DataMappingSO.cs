using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
[CreateAssetMenu()]
public class DataMappingSO : ScriptableObject 
{
    [Serializable]
    public class PortraitData
    {
        public string key;
        public Sprite value;
    }
    [SerializeField]
    public List<PortraitData> portraits;
}
