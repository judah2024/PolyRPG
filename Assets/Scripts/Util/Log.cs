using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using Debug = UnityEngine.Debug;

public class Log
{
    /// <summary> 에러 로그 출력 </summary>
    [Conditional("LOG")]
    static public void Error(string _msg)
    {
        Debug.Log($"<color=red> Error></color> {_msg}");
    }

    /// <summary> 일반 로그 출력 </summary>
    [Conditional("LOG")]
    static public void Massage(string _msg)
    {
        if (EditorPrefs.GetBool("MassageLog", true) == false)
            return;

        Debug.Log($"<color=white> Massage></color> {_msg}");

    }

    /// <summary> Play 로그 출력 </summary>
    [Conditional("LOG")]
    static public void Play(string _msg)
    {
        if (EditorPrefs.GetBool("PlayLog", true) == false)
            return;
        
        Debug.Log($"<color=#40e0d0>Play></color> {_msg}");
    }

    /// <summary> Table 로그 출력 </summary>
    [Conditional("LOG")]
    static public void Table(string _msg)
    {
        Debug.Log($"<color=green>Table></color> {_msg}");
    }

    /// <summary> Server 로그 출력 </summary>
    [Conditional("LOG")]
    static public void Server(string _msg)
    {
        if (EditorPrefs.GetBool("ServerLog", true) == false)
            return;
        
        Debug.Log($"<color=white>Server></color> {_msg}");
    }
}
