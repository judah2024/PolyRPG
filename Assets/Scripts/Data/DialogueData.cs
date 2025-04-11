using System;
using System.Collections;
using System.Collections.Generic;
using EnumDef;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Dialogue/DialogueData", fileName = "new DialogueData")]
public class DialogueData : ScriptableObject
{
    [TextArea(0, 5)] public string[] kArrLine;
    public GameState kNextState;
}
