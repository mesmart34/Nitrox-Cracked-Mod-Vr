using Nitrox.Vr.Common;
using NitroxModel.Logger;
using UnityEngine;

namespace Nitrox.Vr;

extern alias SteamVRRef;
extern alias SteamVRActions;

using SteamVRRef::Valve.VR;
using SteamVRActions::Valve.VR;

public class Controller : MonoBehaviour
{
    private Hand HandType { get; set; }

    public Transform RigTransform { get; set; } = null!;
    
    private SteamVR_Action_Pose actionPose = null!;
    
    private SteamVR_RenderModel steamVRRenderModel = null!;
    
    private Camera eventCamera = null!;

    public void Initialize(Hand handType, Transform rigTransform)
    {
        HandType = handType;
        RigTransform = rigTransform;

        gameObject.layer = LayerID.UI;
        
        gameObject.transform.SetParent(RigTransform);
        
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.localScale = Vector3.one;
        
        InitSteamVrController();
    
        CreateEventCamera();
    }

    private void CreateEventCamera()
    {
        GameObject cameraObject = new GameObject("EventCamera");
        eventCamera = cameraObject.AddComponent<Camera>();
        eventCamera.stereoTargetEye = StereoTargetEyeMask.None;
        eventCamera.nearClipPlane = 0.01f;
        eventCamera.farClipPlane = 10.0f;
        eventCamera.fieldOfView = 1.0f;
        eventCamera.enabled = false;
        cameraObject.transform.SetParent(gameObject.transform);
        cameraObject.transform.localPosition = Vector3.zero;
        cameraObject.transform.localRotation = Quaternion.identity;
        cameraObject.transform.Rotate(45, 0, 0);
    }

    public void SetModelEnabled(bool modelEnabled)
    {
        steamVRRenderModel.enabled = modelEnabled;
    }

    // public void SetPointerEnabled(bool pointerEnabled)
    // {
    //     laserPointer.AliveOrNull()?.SetEnabled(pointerEnabled);
    // }

    public void Update()
    {
        transform.localPosition = actionPose.localPosition;
        transform.localRotation = actionPose.localRotation;
    }

    private void InitSteamVrController()
    {
        SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;

        switch (HandType)
        {
            case Hand.Left:
                inputSource = SteamVR_Input_Sources.LeftHand;
                actionPose = SteamVR_Actions.subnautica_LeftHandPose;
                break;
            case Hand.Right:
                inputSource = SteamVR_Input_Sources.RightHand;
                actionPose = SteamVR_Actions.subnautica_RightHandPose;
                break;
        }

        steamVRRenderModel = gameObject.AddComponent<SteamVR_RenderModel>();
        steamVRRenderModel.SetInputSource(inputSource);

        SteamVR_Behaviour_Pose behaviourPose = gameObject.AddComponent<SteamVR_Behaviour_Pose>();
        behaviourPose.inputSource = inputSource;
        behaviourPose.poseAction = actionPose;
    }
    
    public Camera GetEventCamera()
    {
        return eventCamera;
    }
}
