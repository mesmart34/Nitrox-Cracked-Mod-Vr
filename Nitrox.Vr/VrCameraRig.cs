using System.Collections;
using Nitrox.Vr.Common;
using NitroxModel.Logger;
using UnityEngine;
using UnityEngine.Serialization;

namespace Nitrox.Vr;

extern alias SteamVRRef;
extern alias SteamVRActions;
using SteamVRRef.Valve.VR;

public class VrCameraRig : MonoBehaviour
{
    public static VrCameraRig? Instance;
    public static readonly TransformOffset DefaultTargetTransform = new TransformOffset(Vector3.zero, new Vector3(45, 0, 0));
    
    private TransformOffset targetTransform;
    private GameObject? controllerModelLeft;
    public GameObject? controllerModelRight;
    public GameObject? leftController;
    public GameObject? rightController;
    public GameObject? leftControllerUI;
    public GameObject? rightControllerUI;
    public GameObject? uiRig;
    public GameObject? leftHandTarget;
    public GameObject? rightHandTarget;
    public Transform? rigParentTarget;
    public Camera? uiCamera;
    public Camera? vrCamera;
    
    public TransformOffset TargetTransform
    {
        get
        {
            return targetTransform;
        }
        set
        {
            targetTransform = value;
            // value.Apply(laserPointerUI.transform);
            // value.Apply(laserPointer.transform);
            // value.Apply(laserPointerLeft.transform);
        }
    }

    private void Start()
    {
        Log.Info("VrCameraRig Start");
        SetupControllers();
    }

    public void SetupControllers()
    {
        leftController = new GameObject(nameof(leftController)).WithParent(transform);

        rightController = new GameObject(nameof(rightController)).WithParent(transform);

        leftController.SetActive(false);
        rightController.SetActive(false);
        
        SteamVR_Behaviour_Pose? controller = leftController.AddComponent<SteamVR_Behaviour_Pose>();
        controller.inputSource = SteamVR_Input_Sources.LeftHand;
        controller.poseAction = SteamVRActions.Valve.VR.SteamVR_Actions.subnautica_LeftHandPose;
        controller = rightController.AddComponent<SteamVR_Behaviour_Pose>();
        controller.inputSource = SteamVR_Input_Sources.RightHand;
        controller.poseAction = SteamVRActions.Valve.VR.SteamVR_Actions.subnautica_RightHandPose;
        
        leftController.SetActive(true);
        rightController.SetActive(true);
        
        leftHandTarget = new GameObject(nameof(leftHandTarget)).WithParent(leftController);
        rightHandTarget = new GameObject(nameof(rightHandTarget)).WithParent(rightController);
        leftHandTarget.transform.localEulerAngles = new Vector3(270, 90, 0);
        Vector3 handOffset = new Vector3(90, 270, 0);
        rightHandTarget.transform.localEulerAngles = handOffset;
        
        
        uiRig = new GameObject(nameof(uiRig));
        DontDestroyOnLoad(uiRig);
        
        
        leftControllerUI = new GameObject(nameof(leftControllerUI)).WithParent(uiRig.transform);
        rightControllerUI = new GameObject(nameof(rightControllerUI)).WithParent(uiRig.transform);
        
        leftControllerUI.SetActive(false);
        rightControllerUI.SetActive(false);
        
        controller = leftControllerUI.AddComponent<SteamVR_Behaviour_Pose>();
        controller.inputSource = SteamVR_Input_Sources.LeftHand;
        controller.poseAction = SteamVRActions.Valve.VR.SteamVR_Actions.subnautica_LeftHandPose;
        controller = rightControllerUI.AddComponent<SteamVR_Behaviour_Pose>();
        controller.inputSource = SteamVR_Input_Sources.RightHand;
        controller.poseAction = SteamVRActions.Valve.VR.SteamVR_Actions.subnautica_RightHandPose;
        
        leftControllerUI.SetActive(true);
        rightControllerUI.SetActive(true);
        
        targetTransform = DefaultTargetTransform;
        
        SetupControllerModels();
    }

    public void StealUICamera(Camera camera, bool fromGame = false)
    {
        if (uiRig != null)
        {
            uiRig.transform.SetPositionAndRotation(camera.transform.position, camera.transform.rotation);
            if (uiCamera != null)
            {
                uiCamera.transform.DetachChildren();
                Destroy(uiCamera.gameObject);
            }

            if (fromGame)
            {
                // This fixes a weird issue I had, where the UI Camera from the game would behave like it wasnt moving
                // even though the transform was changing properly.
                // Maybe it is because the tracking was once disabled in the main game, but I am not sure, since I tried enabling it too.
                // Copying the properties from the main camera and setting up the original important properties fixed it.
                uiRig.transform.position = Vector3.zero;
                int oldMask = camera.cullingMask;
                CameraClearFlags oldClear = camera.clearFlags;
                float oldDepth = camera.depth;

                camera.CopyFrom(SNCameraRoot.main.mainCamera);
                camera.transform.localPosition = Vector3.zero;
                camera.transform.localRotation = Quaternion.identity;
                camera.renderingPath = RenderingPath.Forward;
                camera.cullingMask = oldMask;
                camera.clearFlags = CameraClearFlags.Depth;
                camera.depth = oldDepth;

                camera.transform.parent = uiRig.transform;
                camera.transform.localPosition = Vector3.zero;
                camera.transform.localRotation = Quaternion.identity;

                // Set all canvas scalers to static, which makes UI better usable
                FindObjectsOfType<uGUI_CanvasScaler>().Where(obj => !obj.name.Contains("PDA")).ForEach(cs => cs.vrMode = uGUI_CanvasScaler.Mode.Static);
                // SetupPDA();
                // VrQuickSlots = new GameObject("VRQuickSlots").ResetTransform().AddComponent<VRQuickSlots>();
                // VrQuickSlots.Setup(SteamVR_Actions.subnautica_OpenQuickSlotWheel);
            }
            else
            {
                camera.transform.parent = uiRig.transform;
                camera.transform.localPosition = Vector3.zero;
                camera.transform.localRotation = Quaternion.identity;
            }
        }
        uiCamera = camera;
        // VRHud.Setup(uiCamera, rightControllerUI.transform);
    }
    
    public void StealCamera(Camera camera)
    {
        // Destroy/Delete old camera
        // NOTE: Subnautica renderes the water using specific camera component which also renders when the camera is disabled

        if (camera != vrCamera && vrCamera != null)
        {
            vrCamera.enabled = false;
            Destroy(vrCamera.gameObject);
        }

        vrCamera = camera;
        Vector3 oldPos = camera.transform.position;
        transform.position = oldPos;
        vrCamera.transform.parent = transform;

        // AmbientOcclusionVR.AddOcclusionEffect(vrCamera);
    }

    private void LateUpdate()
    {
        if (rigParentTarget == null)
        {
            return;
        }
        
        transform.SetPositionAndRotation(rigParentTarget.position, rigParentTarget.rotation);
        
        if (uiRig == null)
        {
            return;
        }
        
        uiRig.transform.rotation = transform.rotation;
    }

    public void SetupControllerModels()
    {
        Log.Info("SetupControllerModels begin");
        controllerModelLeft = new GameObject(nameof(controllerModelLeft)).ResetTransform();
        controllerModelRight = new GameObject(nameof(controllerModelRight)).ResetTransform();

        controllerModelRight.AddComponent<SteamVR_RenderModel>().SetInputSource(SteamVR_Input_Sources.RightHand);
        controllerModelLeft.AddComponent<SteamVR_RenderModel>().SetInputSource(SteamVR_Input_Sources.LeftHand);
        
        controllerModelLeft.layer = LayerID.UI;
        controllerModelRight.layer = LayerID.UI;

        Settings.AlwaysShowControllersChanged += (_) => { UpdateShowControllers(); };
        Log.Info("SetupControllerModels end");
    }
    
    public void UpdateShowControllers()
    {
        bool inMainMenu = !uGUI.isMainLevel;
        bool alwaysShow = Settings.AlwaysShowControllers;

        if (controllerModelLeft != null)
        {
            controllerModelLeft.SetActive(alwaysShow || inMainMenu);
        }
        
        if (controllerModelRight != null)
        {
            controllerModelRight.SetActive(alwaysShow || inMainMenu);
        }
    }

    public void SetCameraTrackTarget(Transform target)
    {
        rigParentTarget = target;
    }

    public IEnumerator SetupGameCameras()
    {
        VrCameraRig? rig = Instance;
        if (rig != null)
        {
            rig.StealCamera(SNCameraRoot.main.mainCamera);
            yield return new WaitForSeconds(1.0f);
            rig.StealUICamera(SNCameraRoot.main.guiCamera, true);
        }
        yield return new WaitForSeconds(0.1f);

        FindObjectsOfType<uGUI_CanvasScaler>().ForEach(cs => cs.SetDirty());
    }
}
