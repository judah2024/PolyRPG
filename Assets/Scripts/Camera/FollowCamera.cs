using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float kRotateSpeed = 540f;

    private CinemachineVirtualCamera mCamera;
    private CinemachinePOV mPOV;

    [HideInInspector]
    public Transform followTarget;
    private void Awake()
    {
        mCamera = GetComponent<CinemachineVirtualCamera>();
        mPOV = mCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    void Start()
    {
        mCamera.Follow = followTarget;
        mCamera.LookAt = followTarget;
    }

    void Update()
    {
        Vector2 lookInput = InputManager.Instance.LookInput;

        mPOV.m_HorizontalAxis.Value += lookInput.x * kRotateSpeed;
        mPOV.m_VerticalAxis.Value += lookInput.y * kRotateSpeed;
    }
}
