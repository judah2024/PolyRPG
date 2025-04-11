using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class SpringArm : MonoBehaviour
{
    [Header("Follow Setting")]
    public Transform kTarget;
    public float kSmoothSpeed = 0.05f;
    public float kTargetArmLength = 3.0f;
    [FormerlySerializedAs("kMinDistance")] public float kMinArmLength = 0.1f;
    public Vector3 kSocketOffset;
    public Vector3 kTargetOffset;
    Vector3 mCamVelocity;

    [Header("Collision Setting")]
    public float kSphereRadius;
    public float kCollisionSmoothSpeed = 0.05f;
    public LayerMask KCollisionLayerMask;
    float currentArmLength;
    float mArmLengthVelocity;
    
    [Header("Rotation Setting")] 
    public float kMouseSensitivity = 540.0f;
    private Vector2 mCurRotation;

    [Header("Debug Setting")] 
    public bool kDrawDebugging = true;
    public Color kSpringArmColor = Color.red;

    RaycastHit mHitInfo;
    
    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;
    private Camera attachedCamera;

    void LateUpdate()
    {
        if (kTarget == false)
            return;

        Rotate();
        SetSocket();
        CheckCollision();
    }

    void Rotate()
    {
        if (Application.isPlaying)
        {
            float mouseX = Input.GetAxis("Mouse X") * kMouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * kMouseSensitivity * Time.deltaTime;

            mCurRotation.x -= mouseY;
            mCurRotation.y += mouseX;

            mCurRotation.x = Mathf.Clamp(mCurRotation.x, -80, 80);
        }
        
        transform.rotation = Quaternion.Euler(mCurRotation.x, mCurRotation.y, 0);
    }

    void SetSocket()
    {
        Vector3 targetPoint = kTarget.position + kTargetOffset;
        desiredPosition = targetPoint - transform.forward * currentArmLength + kSocketOffset;
        smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref mCamVelocity, kSmoothSpeed);
        transform.position = smoothedPosition;
    }

    void CheckCollision()
    {
        Vector3 targetPoint = kTarget.position + kTargetOffset;
        Vector3 direction = (desiredPosition - targetPoint).normalized;
        
        RaycastHit hit;
        if (Physics.SphereCast(targetPoint, kSphereRadius, direction, out hit, kTargetArmLength, KCollisionLayerMask) ==
            true)
        {
            currentArmLength = Mathf.Clamp(hit.distance, kMinArmLength, kTargetArmLength);
        }
        else
        {
            currentArmLength = Mathf.SmoothDamp(currentArmLength, kTargetArmLength, ref mArmLengthVelocity,
                kCollisionSmoothSpeed);
        }
    }


    void OnDrawGizmosSelected()
    {
        bool isDrawing = kDrawDebugging || kTarget;
        if (isDrawing == false) 
            return;

        Gizmos.color = kSpringArmColor;

        Vector3 start = kTarget.position + kTargetOffset;
        Vector3 end = transform.position + kSocketOffset;

        Gizmos.DrawLine(start, end);
        Gizmos.DrawSphere(start, kSphereRadius);
        Gizmos.DrawSphere(end, kSphereRadius);
    }
}
