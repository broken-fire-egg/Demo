using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    List<TKey> keys;
    [SerializeField]
    List<TValue> values;

    Dictionary<TKey, TValue> target;

    public Dictionary<TKey, TValue> ToDictionary()
    {
        return target;
    }

    public Serialization(Dictionary<TKey, TValue> _target)
    {
        target = _target;
    }

    public Serialization()
    {
        target = new Dictionary<TKey, TValue>();
    }

    public void OnBeforeSerialize()
    {
        keys = new List<TKey>(target.Keys);
        values = new List<TValue>(target.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Mathf.Min(keys.Count, values.Count);
        target = new Dictionary<TKey, TValue>(count);
        for (var i = 0; i < count; ++i)
        {
            target.Add(keys[i], values[i]);
        }
    }

    public void Add(TKey _key, TValue _value)
    {
        target.Add(_key, _value);
    }

    public void Remove(TKey _key)
    {
        target.Remove(_key);
    }

    public bool ContainsKey(TKey _key)
    {
        return target.ContainsKey(_key);
    }

    public bool ContainValue(TValue _value)
    {
        return target.ContainsValue(_value);
    }

    public bool TryGetValue(TKey _key, out TValue _value)
    {
        return target.TryGetValue(_key, out _value);
    }

    public TValue GetValue(TKey _key)
    {
        var enumurator = target.GetEnumerator();
        while (enumurator.MoveNext())
        {
            if (enumurator.Current.Key.Equals(_key))
                return enumurator.Current.Value;
        }

        throw new ArgumentException("No value was found for the given key");
    }

    public TValue GetValue(TValue _value)
    {
        var enumurator = target.GetEnumerator();
        while (enumurator.MoveNext())
        {
            if (enumurator.Current.Value.Equals(_value))
                return enumurator.Current.Value;
        }

        throw new ArgumentException("No value was found for the given key");
    }

    public (TKey, TValue) GetKeyValue(TValue _value)
    {
        var enumurator = target.GetEnumerator();
        while (enumurator.MoveNext())
        {
            if (enumurator.Current.Value.Equals(_value))
                return (enumurator.Current.Key, enumurator.Current.Value);
        }


        throw new ArgumentException("No value was found for the given key");
    }


    public IEnumerator GetEnumerator()
    {
        return target.GetEnumerator();
    }
}