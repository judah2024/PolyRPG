using System;
using System.Collections;
using System.Collections.Generic;
using EnumDef;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIDialogue : MonoBehaviour
{
    public RectTransform kDialoguePanel;
    public TMP_Text kDialogueText;
    public DialogueCamera kDialogueCamera;

    [Header("UI Setting")]
    [Range(0, 100)] public float kTypingSpeed = 5f;
    public float kMoveTime = 0.5f;

    Vector2 mOpenPos;
    Vector2 mClosePos;

    bool mIsTyping = false;
    bool mIsMovingSentence = false;
    string mCurSentence;
    Coroutine mTypingCoroutine;

    void Awake()
    {
        mOpenPos = kDialoguePanel.anchoredPosition;
        mClosePos = mOpenPos + Vector2.down * 500f;
    }

    void Update()
    {
            if (Input.GetMouseButtonDown(0) == true)
            {
                OnDialogueClick();
            }
    }
    
    public void OnShopNpcDialogFinish()
    {
    }

    public void OnNextStage()
    {
        //Mng.globalCanvas.loading.Open();
    }

    public void StartDialogue(DialogueData dialogueData, Transform other, UnityAction onDialogComplete)
    {
        gameObject.SetActive(true);
        kDialogueCamera.StartDialogue(Mng.play.kPlayer.transform, other);
        Mng.play.state = GameState.Event;
        kDialogueText.text = "";
        kDialoguePanel.anchoredPosition = mClosePos;
        LeanTween.move(kDialoguePanel, mOpenPos, kMoveTime)
            .setOnComplete(_ => StartCoroutine(ProcessDialogue(dialogueData, onDialogComplete)));
    }
    
    public void EndDialogue(GameState nextState)
    {
        LeanTween.move(kDialoguePanel, mClosePos, kMoveTime).setOnComplete(_ =>
        {
            Mng.play.state = nextState;
//            Debug.Log("Change State!");
            if (nextState == GameState.Shop)
            {
                OnShopNpcDialogFinish();
            }
            
            kDialogueCamera.EndDialogue();
        });
    }

    void OnDialogueClick()
    {
        if (mIsTyping == true)
        {
            mIsTyping = false;
        }
        else
        {
            mIsMovingSentence = true;
        }
    }

    IEnumerator ProcessDialogue(DialogueData dialogueData, UnityAction onDialogComplete)
    {
        foreach (string sentence in dialogueData.kArrLine)
        {
            mIsMovingSentence = false;
            mCurSentence = sentence;
            mTypingCoroutine = StartCoroutine(TypingCoroutine(sentence));
            yield return mTypingCoroutine;

            yield return new WaitUntil(() => Mng.play.state != GameState.Pause && mIsMovingSentence == true);
        }

        onDialogComplete?.Invoke();
    }

    IEnumerator TypingCoroutine(string sentence)
    {
        mIsTyping = true;
        kDialogueText.text = "";

        int index = 0;
        float time = 0f;
        while (index < sentence.Length)
        {
            if (mIsTyping == false)
                break;

            if (kTypingSpeed == 0f)
                break;
            
            float waitSecond = ConstDef.MAX_TYPE_DELAY * kTypingSpeed * 0.01f;
            
            if (time >= waitSecond)
            {
                index++;
                kDialogueText.text = sentence.Substring(0, index);
                time -= waitSecond;
            }

            time += Time.deltaTime;
            yield return null;
        }

        kDialogueText.text = sentence;
        yield return new WaitForSeconds(0.1f);
        mIsTyping = false;
    }


}
