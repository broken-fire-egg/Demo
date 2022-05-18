using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling<T> : MonoBehaviour
{
    public int defaultCap;
    public GameObject origin;
    public List<GameObject> gameObjects;
    //public List<Transform> transforms;
    //public List<T> components;
    public void Start()
    {
        for (int i = 0; i < defaultCap; i++)
        {
            var newgo = Instantiate(origin, gameObject.transform);
            gameObjects.Add(newgo);

        }

    }

    public GameObject GetGameObjectFromOP()
    {
        foreach(GameObject go in gameObjects)
        {
            if (!go.activeInHierarchy)
                return go;
        }
        return null;
    }


    public T GetComponentFromOP()
    {
        foreach (GameObject go in gameObjects)
        {
            if (!go.activeInHierarchy)
                return go.GetComponent<T>();
        }
        return default(T);
    }
    public Transform GetTransformFromOP()
    {
        foreach (GameObject go in gameObjects)
        {
            if (!go.activeInHierarchy)
                return go.transform;
        }
        return null;
    }


}
