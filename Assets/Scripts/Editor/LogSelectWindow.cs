using UnityEditor;
using UnityEngine;

public class LogSelectWindow : EditorWindow
{
    private string myString = "Hello World";
    private bool myBool = true;
    private float myFloat = 1.23f;

    [MenuItem("Window/Log Select")]
    public static void ShowWindow()
    {
        GetWindow<LogSelectWindow>("Log Select");
    }

    void OnGUI()
    {
        var isPlayLog = EditorPrefs.GetBool("PlayLog", true);
        var check = EditorGUILayout.Toggle("Play 로그", isPlayLog);
        EditorPrefs.SetBool("PlayLog", check);
        
        var isServerLog = EditorPrefs.GetBool("ServerLog", true);
        check = EditorGUILayout.Toggle("Server 로그", isServerLog);
        EditorPrefs.SetBool("ServerLog", check);


        var isTableLog = EditorPrefs.GetBool("TableLog", true);
        check = EditorGUILayout.Toggle("Table 로그", isServerLog);
        EditorPrefs.SetBool("TableLog", check);

        var isMassageLog = EditorPrefs.GetBool("MassageLog", true);
        check = EditorGUILayout.Toggle("Massage 로그", isServerLog);
        EditorPrefs.SetBool("MassageLog", check);

        /*
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, 0, 10);
        */


    }
}
