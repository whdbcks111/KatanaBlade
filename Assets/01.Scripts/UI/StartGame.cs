using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public string sceneName;
    public void StartGameLoading()
    {
        LoadingSceneManager.Instance.LoadScene(sceneName);
    }
}
