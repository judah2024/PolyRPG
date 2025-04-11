using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    public static UILoading Instance;

    void Awake()
    {
        Instance = this;
        kFillImage.fillAmount = 0;
        gameObject.SetActive(false);
    }

    public CanvasGroup kCanvasGroup;
    public Image kFillImage;
    public float kFadeTime = 3f;
    public float kFakeLoadingTime = 2f;

    public void Loading()
    {
        StartCoroutine(CoLoading());
    }

    public void UpdateFillImage(float ratio)
    {
        kFillImage.fillAmount = ratio;
    }

    IEnumerator CoLoading()
    {
        yield return CoFade(true);
        yield return new WaitForSeconds(0.25f);
        yield return CoFade(false);
    }

    public IEnumerator CoFade(bool isFadeIn)
    {
        gameObject.SetActive(true);

        float time = 0f;
        while (time <= kFadeTime)
        {

            yield return null;
            time += Time.unscaledDeltaTime * kFadeTime;
            if (isFadeIn == true)
                kCanvasGroup.alpha = Mathf.Lerp(0, 1, time / kFadeTime);
            else
                kCanvasGroup.alpha = Mathf.Lerp(1, 0, time / kFadeTime);
        }

        if (isFadeIn == false)
            gameObject.SetActive(false);
    }
}
