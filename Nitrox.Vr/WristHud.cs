using Nitrox.Vr.Common;
using NitroxModel.Logger;
using UnityEngine;

namespace Nitrox.Vr;

public static class WristHud
{
    private static TransformOffset wristOffset = new TransformOffset(new Vector3(-0.079f, 0.148f, -0.158f), new Vector3(350.494f, 88.400f, 244.161f));
    private static GameObject wristTarget;
    private static Canvas canvas;
    private static CanvasGroup canvasGroup;

    // Cached Values
    private static Transform hudContent;
    private static Transform uiCamera;
    private static Transform cachedIndexTip;
    private static FMODAsset turnOnSound;
    private static FMODAsset turnOffSound;

    // State
    public static bool isHudOn = true;
    private static bool touchingWrist = false;
    private static bool prevTouchingWrist = false;

    private static FMODAsset CreateFMODAsset(string eventPath)
    {
        FMODAsset asset = ScriptableObject.CreateInstance<FMODAsset>();
        asset.path = eventPath;
        return asset;
    }

    // Create Wrist World Canvas
    public static void Setup()
    {
        VrCameraRig? rig = VrCameraRig.Instance;
        if (rig != null)
        {
            if (rig.uiCamera != null)
            {
                uiCamera = rig.uiCamera.transform;
            }
            hudContent = uGUI.main.hud.transform.GetChild(0);

            if (wristTarget == null)
            {
                if (rig.leftControllerUI != null)
                {
                    wristTarget = new GameObject("WristTarget").WithParent(rig.leftControllerUI).ResetTransform();
                }
                if (wristTarget != null)
                {
                    GameObject wristCanvasGo = new GameObject("WristCanvas").WithParent(wristTarget).ResetTransform();
                    canvas = wristCanvasGo.CreateWorldCanvas();
                    canvasGroup = wristCanvasGo.AddComponent<CanvasGroup>();
                    wristCanvasGo.transform.localScale = new Vector3(0.0004f, 0.0004f, 0.0004f);
                }
                if (wristTarget != null)
                {
                    wristOffset.Apply(wristTarget.transform);
                }
            }
        }

        Settings.PutBarsOnWristChanged -= OnPutBarsOnHandChanged;
        Settings.PutBarsOnWristChanged += OnPutBarsOnHandChanged;
        Toggle(Settings.PutBarsOnWrist);

        turnOnSound = CreateFMODAsset("event:/tools/flashlight/turn_on");
        turnOffSound = CreateFMODAsset("event:/tools/flashlight/turn_off");
    }

    private static Transform GetIndexFingerTip()
    {
        if (cachedIndexTip != null)
        {
            return cachedIndexTip;
        }
        Animator? animator = Player.main.playerAnimator;
        if (animator is Animator anim)
        {
            Transform? tip = anim.transform.Find("export_skeleton/head_rig/neck/chest/clav_R/clav_R_aim/shoulder_R/hand_R/hand_R_point_base/hand_R_point_mid/hand_R_point_tip_rig");
            if (tip != null)
            {
                cachedIndexTip = tip;
                return tip;
            }
        }
        return null;
    }

    private static void OnPutBarsOnHandChanged(bool isOn)
    {
        Toggle(isOn);
    }

    private static void OnUpdate()
    {
        if (!uGUI.isMainLevel)
        {
            return;
        }
        Vector3 camPos = uiCamera.transform.position;
        
        if (VrCameraRig.Instance == null)
        {
            return;
        }
        
        if (VrCameraRig.Instance.rigParentTarget == null)
        {
            return;
        }
        
        Vector3 worldRigPos = VrCameraRig.Instance.rigParentTarget.position;
        Vector3 wristPos = wristTarget.transform.position;

        Vector3 wristDir = wristTarget.transform.TransformDirection(Vector3.forward);
        Vector3 toCam = (wristPos - camPos).normalized;

        float wristCamDot = Vector3.Dot(wristDir, toCam);
        bool isFacingCamera = wristCamDot > 0.1f;
        // DebugPanel.Show($"dot = {dot} <= {wristDir}, {toCam}");
        canvasGroup.alpha = Mathf.Max(wristCamDot, 0.0f);

        if (isFacingCamera && GetIndexFingerTip() is Transform indexTip)
        {
            Vector3 uiIndexPos = indexTip.position - worldRigPos;
            float wristDistance = Vector3.Distance(uiIndexPos, wristPos);
            // DebugPanel.Show($"wristDistance = {wristDistance} <= uiPos{uiIndexPos}, {wristPos}");
            const float THRESHOLD = 0.1f;
            touchingWrist = wristDistance < THRESHOLD;
            if (touchingWrist && !prevTouchingWrist)
            {
                isHudOn = !isHudOn;
                Utils.PlayFMODAsset(isHudOn ? turnOnSound : turnOffSound);
            }
            prevTouchingWrist = wristDistance < THRESHOLD;
        }
    }

    private static void Toggle(bool isOn)
    {
        if (canvas == null)
        {
            Setup();
        }

        GameObject? barsPanel = uGUI.main.barsPanel;
        if (isOn)
        {
            // Move to wrist
            Log.Debug("Turning WristHud on");
            if (canvas != null)
            {
                barsPanel.WithParent(canvas.transform).ResetTransform();
            }
            barsPanel.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            ManagedUpdate.Subscribe(ManagedUpdate.Queue.PreCanvasFirst, new ManagedUpdate.OnUpdate(OnUpdate));
        }
        else
        {
            // Move back
            Log.Debug("Turning WristHud off");
            barsPanel.transform.SetParent(hudContent.transform, false);
            barsPanel.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            barsPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
            ManagedUpdate.Unsubscribe(ManagedUpdate.Queue.PreCanvasFirst, new ManagedUpdate.OnUpdate(OnUpdate));
            isHudOn = true;
        }
    }
}
