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
    private static readonly TransformOffset DefaultTargetTransform = new TransformOffset(Vector3.zero, new Vector3(45, 0, 0));
    
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
    public LaserPointer? laserPointer;
    public LaserPointer? laserPointerLeft;
    public LaserPointer? laserPointerUI;
    public GameObject? worldTarget;
    public float worldTargetDistance;
    
    public TransformOffset TargetTransform
    {
        set
        {
            targetTransform = value;
            if (laserPointerUI != null)
            {
                value.Apply(laserPointerUI.transform);
            }
            if (laserPointer != null)
            {
                value.Apply(laserPointer.transform);
            }
            if (laserPointerLeft != null)
            {
                value.Apply(laserPointerLeft.transform);
            }
        }
    }

    public Camera? UIControllerCamera
    {
        get
        {
            return laserPointerUI == null ? null : laserPointerUI.eventCamera;
        }
    }
    public Camera? WorldControllerCamera
    {
        get
        {
            return laserPointer == null ? null : laserPointer.eventCamera;
        }
    }

    private void Start()
    {
        Log.Info("VrCameraRig Start");
        SetupControllers();
        
        StartCoroutine(DelayedRecenter(1.0f));
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
        
        laserPointer = new GameObject(nameof(laserPointer)).WithParent(rightController.transform).AddComponent<LaserPointer>();
        laserPointerLeft = new GameObject(nameof(laserPointerLeft)).WithParent(leftController.transform).AddComponent<LaserPointer>();
        laserPointerLeft.gameObject.SetActive(false);
        laserPointer.disableAfterCreation = true;
        
        uiRig = new GameObject(nameof(uiRig));
        DontDestroyOnLoad(uiRig);
        
        
        leftControllerUI = new GameObject(nameof(leftControllerUI)).WithParent(uiRig.transform);
        rightControllerUI = new GameObject(nameof(rightControllerUI)).WithParent(uiRig.transform);
        laserPointerUI = new GameObject(nameof(laserPointerUI)).WithParent(rightControllerUI.transform).AddComponent<LaserPointer>();
        laserPointerUI.doWorldRaycasts = true;
        laserPointerUI.useUILayer = true;
        
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
        
        FPSInputModule? fpsInput = FindObjectOfType<FPSInputModule>();
        laserPointer.inputModule = fpsInput;
        laserPointerLeft.inputModule = fpsInput;
        laserPointerUI.inputModule = fpsInput;
    }

    private IEnumerator DelayedRecenter(float delay)
    {
        yield return new WaitForSeconds(delay);
        VRUtil.Recenter();
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
                SetupPDA();
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
        if (rightControllerUI != null)
        {
            VrHud.Setup(uiCamera, rightControllerUI.transform);
        }
    }

    private void SetupPDA()
    {
        // Move the quickslots to bottom of PDA bottom left and make it bigger
        uGUI_PDA? pda = uGUI_PDA.main;
        Transform targetParent = pda.tabInventory.transform;
        uGUI_QuickSlots? qs = FindObjectOfType<uGUI_QuickSlots>();
        Transform qstf = qs.transform;

        qstf.parent = targetParent;
        qstf.localPosition = new Vector3(-250, -455, 4f);
        qstf.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        qstf.localRotation = Quaternion.identity;

        // Add Pasuse Menu Button to PDA to PDA
        uGUI_Dialog? dialog = pda.GetComponentInChildren<uGUI_Dialog>(true);
        uGUI_DialogButton? buttonPrefab = dialog.buttonPrefab;
        uGUI_DialogButton? button = Instantiate(buttonPrefab, targetParent).GetComponent<uGUI_DialogButton>();
        button.button.transform.parent = targetParent;
        button.button.gameObject.gameObject.name = "PauseMenuButton";
        button.text.text = "Pause Menu";
        button.button.onClick.RemoveAllListeners();
        button.button.onClick.AddListener(() =>
        {
            IngameMenu.main.Open();
        });
        // Move it to the bottom right
        button.rectTransform.anchoredPosition = new Vector2(1100, 50);
        button.rectTransform.pivot = new Vector2(1, 0);
        button.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
        button.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
        button.rectTransform.ForceUpdateRectTransforms();
        button.rectTransform.GetComponentsInChildren<RectTransform>().ForEach(rt => rt.ForceUpdateRectTransforms());
    }

    public void SetWorldTarget(GameObject activeTarget, float activeHitDistance)
    {
        worldTarget = activeTarget;
        worldTargetDistance = activeHitDistance;
        if (laserPointerUI != null)
        {
            laserPointerUI.SetWorldTarget(worldTarget, worldTargetDistance);
        }
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
        Log.Info("UpdateShowControllers begin");
      
        bool inMainMenu = !uGUI.isMainLevel;
        bool alwaysShow = Settings.AlwaysShowControllers;

        if (controllerModelLeft != null)
        {
            Log.Info("UpdateShowControllers LEFT");
            controllerModelLeft.SetActive(alwaysShow || inMainMenu);
        }
        
        if (controllerModelRight != null)
        {
            Log.Info("UpdateShowControllers RIGHT");
            controllerModelRight.SetActive(alwaysShow || inMainMenu);
        }
        Log.Info("UpdateShowControllers end");
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
            Log.Info("SetupGameCameras IS WORKING");
        }
        yield return new WaitForSeconds(0.1f);

        FindObjectsOfType<uGUI_CanvasScaler>().ForEach(cs => cs.SetDirty());
    }

    public static Transform? GetTargetTransform()
    {
        if (Instance == null)
        {
            return null;
        }
        
        if (Instance.laserPointer != null)
        {
            return Instance.laserPointer.transform;
        }

        return null;
    }
    
    public static Transform? GetLeftTargetTransform()
    {
        if (Instance == null)
        {
            return null;
        }
        
        if (Instance.laserPointerLeft != null)
        {
            return Instance.laserPointerLeft.transform;
        }

        return null;
    }
}
