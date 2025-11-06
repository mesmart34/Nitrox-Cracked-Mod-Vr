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
    public Camera EventCamera { get; private set; } = null!;

    public void Initialize(Hand handType, Transform rigTransform)
    {
        HandType = handType;
        RigTransform = rigTransform;
        
        gameObject.transform.SetParent(RigTransform);

        gameObject.transform.Reset();
        
        InitSteamVrController();
    
        CreateEventCamera();
    }

    private void CreateEventCamera()
    {
        GameObject cameraObject = new(nameof(EventCamera));
        EventCamera = cameraObject.AddComponent<Camera>();
        EventCamera.stereoTargetEye = StereoTargetEyeMask.None;
        EventCamera.nearClipPlane = 0.01f;
        EventCamera.farClipPlane = 10.0f;
        EventCamera.fieldOfView = 1.0f;
        EventCamera.enabled = false;
        
        cameraObject.transform.SetParentAndReset(gameObject.transform);
        cameraObject.transform.Rotate(45, 0, 0);
    }

    
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

    public void SetLayer(int layerId)
    {
        gameObject.layer = layerId;
        steamVRRenderModel.useGUILayout = layerId == LayerID.UI;
    }
}
