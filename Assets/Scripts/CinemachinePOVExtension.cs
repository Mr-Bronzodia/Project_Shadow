using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class CinemachinePOVExtension : CinemachineExtension
{
    private StarterAssetsInputs _inputs;


    protected override void Awake()
    {
        base.Awake();

        _inputs = GetComponentInParent<StarterAssetsInputs>();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {

            }
        }
    }
}
