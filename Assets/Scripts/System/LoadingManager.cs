using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void SceneLoad(string scenename)
    {
        SceneManager.LoadSceneAsync(scenename);

    }


}
