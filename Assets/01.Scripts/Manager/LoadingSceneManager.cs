using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class LoadingSceneManager : MonoBehaviour
{

    private static LoadingSceneManager _instance = null;


    private LoadingUI _uiPrefab;
    public static LoadingSceneManager Instance
    {
        get
        {
            if(_instance is null || _instance.Equals(null))
            {
                _instance = new GameObject(nameof(LoadingSceneManager)).AddComponent<LoadingSceneManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _uiPrefab = Resources.Load<LoadingUI>("UI/LoadingUI");
    }

    public void LoadScene(string sceneName)
    {
        print("Start..");
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    public IEnumerator LoadSceneRoutine(string sceneName)
    {
        LoadingUI ui = Instantiate(_uiPrefab);
        ui.Title.SetText("¾À ·ÎµùÁß...");
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        float showProgress = op.progress;
        float vel = 1;
        while (!op.isDone && showProgress < 0.99f)
        {
            showProgress = Mathf.SmoothDamp(showProgress, op.progress / 0.9f, ref vel, 0.3f);
            print("Load : " + op.progress);
            ui.ProgressText.SetText(string.Format("{0:0.0}%", showProgress * 100));
            ui.LoadingImage.fillAmount = showProgress;
            yield return null;
        }

        op.allowSceneActivation = true;
    }
}
