using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private IEnumerator Start()
    {
        Log.Error("에러");
        Log.Table("테이블");
        Log.Server("서버");
        Log.Massage("메시지");
        Log.Play("플레이");
        
        var globalCanvas = FindObjectOfType<GlobalCanvas>(true);
        globalCanvas.gameObject.SetActive(true);
        yield return new WaitUntil( () => Mng.globalCanvas != null);
        yield return null;
        
        var sound = GetComponentInChildren<SoundManager>(true);
        sound.gameObject.SetActive(true);
        yield return new WaitUntil( () => Mng.sound != null);
        yield return null;
        
        var table = GetComponentInChildren<TableManager>(true);
        table.gameObject.SetActive(true);
        yield return new WaitUntil( () => Mng.table != null);
        yield return null;
        
        var data = GetComponentInChildren<DataManager>(true);
        data.gameObject.SetActive(true);
        yield return new WaitUntil( () => Mng.data != null);
        yield return null;
        
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == StrDef.NAME_PLAY_SCENE);

        var canvas = FindObjectOfType<MainCanvas>(true);
        canvas.gameObject.SetActive(true);
        yield return new WaitUntil( () => Mng.canvas != null);
        yield return null;
        
        var play = FindObjectOfType<PlayManager>(true);
        play.gameObject.SetActive(true);
        yield return new WaitUntil( () => Mng.play != null);
        yield return null;
    }

    #region Loading

    public float kFakeLoadingTime = 2.0f;
    string mLoadSceneName;

    public void LoadScene(string sceneName)
    {
        Mng.globalCanvas.loading.gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        mLoadSceneName = sceneName;
        
        StartCoroutine(CoLoadScene());
    }

    
    IEnumerator CoLoadScene()
    {
        yield return Mng.globalCanvas.loading.CoFade(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(mLoadSceneName);
        operation.allowSceneActivation = false;

        float time = 0f;
        while (operation.isDone == false)
        {
            yield return null;

            if (operation.progress < 0.9f)
            {
                Mng.globalCanvas.loading.UpdateFillImage(operation.progress);
            }
            else
            {
                time += Time.unscaledDeltaTime;
                Mng.globalCanvas.loading.UpdateFillImage(time / kFakeLoadingTime);
                if (time >= kFakeLoadingTime)
                {
                    operation.allowSceneActivation = true;
                    break;
                }
            }
        }

    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name == mLoadSceneName)
        {
            StartCoroutine(Mng.globalCanvas.loading.CoFade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    #endregion
    
}
