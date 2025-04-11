using System;
using System.Collections;
using System.Collections.Generic;
using EnumDef;
using UnityEngine;

public class UIPause : MonoBehaviour
{
    public GameObject kOptionObj;
    public GameObject kTitleObj;
    public GameObject kExitObj;

    public float kFadeDuration = 1;
    public float kDelay = 0.1f;

    private void OnEnable()
    {
        kOptionObj.SetActive(false);
        kTitleObj.SetActive(false);
        kExitObj.SetActive(false);

        Mng.play.state = GameState.Pause;
        StartCoroutine(CoObjectTween());
    }

    private void OnDisable()
    {
        Mng.play.state = GameState.Playing;
        StopAllCoroutines();
    }

    IEnumerator CoObjectTween()
    {
        FadeObject(kOptionObj);
        yield return new WaitForSeconds(kDelay);

        FadeObject(kTitleObj);
        yield return new WaitForSeconds(kDelay);

        FadeObject(kExitObj);
    }

    void FadeObject(GameObject _obj)
    {
        _obj.SetActive(true);
        CanvasGroup group = _obj.GetComponent<CanvasGroup>();

        group.alpha = 0;
        LeanTween.alphaCanvas(group, 1, kFadeDuration);

        RectTransform rect = _obj.GetComponent<RectTransform>();

        Vector2 to = rect.anchoredPosition;
        Vector2 from = to;
        from.y -= 50;

        rect.anchoredPosition = from;
        LeanTween.move(rect, to, kFadeDuration);
    }

    public void OnOptionButtonClick()
    {
        Mng.globalCanvas.setting.SetActive(true);
    }
    
    public void OnTitleClick()
    {
        Mng.sound.PlaySFX(SFXType.UI_Click);
        Manager.Instance.LoadScene("Title_Dungeon");
    }

    public void OnExitButtonClick()
    {
        gameObject.SetActive(false);
    }
}
