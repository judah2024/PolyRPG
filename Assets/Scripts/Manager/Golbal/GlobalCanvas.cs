using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCanvas : MonoBehaviour
{
    static public GlobalCanvas Instance;

    public GameObject setting;
    public UILoading loading { get; private set; }
    
    public UIMessageBox messageBox { get; private set; }

    private void Awake()
    {
        messageBox = GetComponentInChildren<UIMessageBox>(true);
        
        loading = GetComponentInChildren<UILoading>(true);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }
}
