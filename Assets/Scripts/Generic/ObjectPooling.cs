using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling<T> : MonoBehaviour
{
    public class PoolObject
    {
        public T component;
        public GameObject gameObject;
        public PoolObject(T component, GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.component = component;
        }
    }
    public int defaultCap;
    public GameObject origin;
    public List<PoolObject> poolObjects;
    public void Start()
    {
        poolObjects = new List<PoolObject>();
        for (int i = 0; i < defaultCap; i++)
        {
            var newgo = Instantiate(origin, gameObject.transform);
            poolObjects.Add(new PoolObject(newgo.GetComponent<T>(), newgo.gameObject));
        }

    }
    public PoolObject GetRestingPoolObject()
    {
        foreach (var po in poolObjects)
        {
            if (!po.gameObject.activeInHierarchy)
                return po;
        }
        return null;

    }

}
