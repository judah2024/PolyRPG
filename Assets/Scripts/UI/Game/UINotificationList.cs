using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UINotificationList : MonoBehaviour
{
    private const float SPACING = 10f;
    
    public GameObject kNotifyPrefab;
    public float kShowDuration = 1f;
    public float kFadeDuration = 0.5f;
    public float kMoveDuration = 0.5f;

    private List<GameObject> mActiveUIObjectList = new List<GameObject>();

    //UINotification.Push("룰루랄라");  // ???
    
    public void ShowNotification(string message)
    {
        GameObject obj = Instantiate(kNotifyPrefab, transform);
        mActiveUIObjectList.Add(obj);
        
        // 알림 초기화
        UINotification ui = obj.GetComponent<UINotification>();
        ui.kContentText.text = message;

        UpdatePositions();
        RectTransform rect = obj.GetComponent<RectTransform>();
        Vector2 from = rect.anchoredPosition;
        from.x = Screen.width;


        rect.anchoredPosition = from;
        LeanTween.moveX(rect, 0, kMoveDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(_ =>
            {
                StartCoroutine(CoFadeOut(obj));
            });
    }

    IEnumerator CoFadeOut(GameObject obj)
    {
        yield return new WaitForSeconds(kShowDuration);
        
        // 페이드아웃
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(canvasGroup, 0, kFadeDuration)
            .setOnComplete(() => {
                mActiveUIObjectList.Remove(obj);
                UpdatePositions();
                Destroy(obj);
            });
    }

    void UpdatePositions()
    {
        float y = 0;

        foreach (var obj in mActiveUIObjectList)
        {
            RectTransform rect = obj.GetComponent<RectTransform>();
            Vector2 newPos = rect.anchoredPosition;
            newPos.y = y;

            rect.anchoredPosition = newPos;

            y -= (rect.rect.height + SPACING);
        }
    }
}
