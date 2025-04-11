using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DialogueCamera : MonoBehaviour
{
    public float kBlendTime = 0.5f;

    public float kHeight = 1.6f;
    public float kDistance = 3f;
    public float kAngleOffset = 30f;

    [HideInInspector]
    public bool isActive;

    private CinemachineVirtualCamera mCamera;
    private GameObject mCenterPoint;
    private CinemachineComposer mComposer;

    private void Awake()
    {
        mCamera = GetComponent<CinemachineVirtualCamera>();
        mCenterPoint = new GameObject("Middle Point");

        mComposer = mCamera.GetCinemachineComponent<CinemachineComposer>();
        if (mComposer == null)
        {
            mComposer = mCamera.AddCinemachineComponent<CinemachineComposer>();
            mComposer.m_TrackedObjectOffset = new Vector3(0, kHeight, 0);
            mComposer.m_LookaheadTime = 0;
            mComposer.m_HorizontalDamping = 0.5f;
            mComposer.m_VerticalDamping = 0.5f;
        }

        mCamera.Priority = 0;
    }

    public void StartDialogue(Transform player, Transform other)
    {
        Vector3 centerPos = (player.position + other.position) * 0.5f;
        mCenterPoint.transform.position = centerPos;
        
        Vector3 direction = (player.position - other.position).normalized;
        if (player == other)
        {
            direction = player.forward;
        }

        Vector3 cameraOffset = Quaternion.Euler(0, kAngleOffset, 0) * direction * kDistance;
        cameraOffset.y = kHeight;

        mCamera.Follow = mCenterPoint.transform;
        mCamera.LookAt = mCenterPoint.transform;

        CinemachineTransposer transposer = mCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {
            transposer.m_FollowOffset = cameraOffset;
        }

        mCamera.Priority = 11;
        isActive = true;
    }

    public void EndDialogue()
    {
        mCamera.Priority = 0;
        isActive = false;
    }

    private void OnDrawGizmos()
    {
        if (mCenterPoint != null)
            Gizmos.DrawWireSphere(mCenterPoint.transform.position, 0.25f);
    }
}
