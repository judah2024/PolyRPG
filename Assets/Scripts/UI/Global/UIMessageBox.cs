using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class UIMessageBox : MonoBehaviour
{
    public GameObject kConfirmButton;
    public GameObject kYesButton;
    public GameObject kNoButton;

    public TMP_Text kTitleText;
    public TMP_Text kContentText;
    
    
    private Action mConfirmAct;
    private Action mYesAct;
    private Action mNoAct;
    
    //확인 버튼 1개짜리 메세지 박스
    static public void Open(string _title, string _content, Action _confirmAct)
    {
        Mng.globalCanvas.messageBox.gameObject.SetActive(true);
        Mng.globalCanvas.messageBox.OpenOk(_title, _content, _confirmAct);
    }
    
    //확인 버튼 1개짜리 메세지 박스
    static public void Open(string _title, string _content, Action _yesAct, Action _noAct)
    {
        Mng.globalCanvas.messageBox.gameObject.SetActive(true);
        Mng.globalCanvas.messageBox.OpenYesNo(_title, _content, _yesAct, _noAct);
    }
    
    public void OpenOk(string _title, string _content, Action _confirmAct)
    {
        kConfirmButton.SetActive(true);
        kYesButton.SetActive(false);
        kNoButton.SetActive(false);

        kTitleText.text = _title;
        kContentText.text = _content;
        
        mConfirmAct = _confirmAct;
    }

    public void OpenYesNo(string _title, string _content, Action _yesAct, Action _noAct)
    {
        kConfirmButton.SetActive(false);
        kYesButton.SetActive(true);
        kNoButton.SetActive(true);
        
        kTitleText.text = _title;
        kContentText.text = _content;
        
        mYesAct = _yesAct;
        mNoAct = _noAct;
    }

    public void OnConfirmButtonClick()
    {
        mConfirmAct?.Invoke();
        gameObject.SetActive(false);
    }
    
    public void OnYesButtonClick()
    {
        mYesAct?.Invoke();
        gameObject.SetActive(false);
    }
    
    public void OnNoButtonClick()
    {
        mNoAct?.Invoke();
        gameObject.SetActive(false);
    }
}
