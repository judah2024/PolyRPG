using System;
using System.Collections;
using System.Collections.Generic;
using EnumDef;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UITitleButtonPanel : MonoBehaviour
{
    [Header("시간")]
    public float kAnimationDuration = 1.5f;
    public float kOrderDelay = 0.25f;

    List<RectTransform> mListButton;

    void Awake()
    {
        mListButton = new List<RectTransform>(transform.childCount);
        foreach (RectTransform child in transform)
        {
            mListButton.Add(child);
        }
    }
    
    void Start()
    {
        for (int i = 0; i < mListButton.Count; i++)
        {
            float delay = 0.5f + kOrderDelay * i;
            
            Button button = mListButton[i].GetComponent<Button>();
            button.interactable = false;

            mListButton[i].localScale = Vector3.zero;
            LeanTween.scale(mListButton[i], Vector3.one, kAnimationDuration)
                .setDelay(delay).setEase(LeanTweenType.easeOutBounce)
                .setOnComplete(() =>
                {
                    button.interactable = true;
                });
        }
    }

    public void OnStartButtonClick()
    {
        Mng.sound.PlaySFX(SFXType.UI_Click);
        Manager.Instance.LoadScene(StrDef.NAME_PLAY_SCENE);
    }

    public void OnOptionButtonClick()
    {
        Mng.globalCanvas.setting.SetActive(true);
    }
}
