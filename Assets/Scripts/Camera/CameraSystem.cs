using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public FollowCamera kPlayerCamera;
    public DialogueCamera kDialogueCamera;
    public Transform kPlayerTransform;

    private void Awake()
    {
        kPlayerCamera.followTarget = kPlayerTransform;
    }
}
