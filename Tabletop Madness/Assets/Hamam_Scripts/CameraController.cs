using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{    
    public Transform player, enemy, normalDice, specialDice;
    public CinemachineVirtualCamera followCamera, rotatingCamera;
    public float rotationSpeed = 1;

    private CinemachineTrackedDolly trackedDolly;
    private bool isRotating = false;

    private void Awake()
    {
        trackedDolly = rotatingCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }
    public enum LookTarget
    {
        Player,
        Enemy,
        NormalDice,
        SpecialDice
    };

    private Transform targetTransform;

    void Start()
    {
        targetTransform = player;
    }
   
    void LateUpdate()
    {
        if(isRotating)
        {
            trackedDolly.m_PathPosition += rotationSpeed * Time.unscaledDeltaTime;
        }
    }

    public void SwitchToFollow()
    {
        followCamera.Priority = 1;
        rotatingCamera.Priority = 0;
        isRotating = false;
    }
    public void SwitchToRotate()
    {
        followCamera.Priority = 0;
        rotatingCamera.Priority = 1;
        isRotating = true;
    }


    public void LookAtTarget(LookTarget target)
    {
        switch(target)
        {
            case LookTarget.Player:
                targetTransform = player;                
                break;
            case LookTarget.Enemy:
                targetTransform = enemy;
                break;
            case LookTarget.NormalDice:
                targetTransform = normalDice;
                break;
            case LookTarget.SpecialDice:
                targetTransform = specialDice;
                break;
        }

        followCamera.Follow = targetTransform;
        followCamera.LookAt = targetTransform;
    }
}
