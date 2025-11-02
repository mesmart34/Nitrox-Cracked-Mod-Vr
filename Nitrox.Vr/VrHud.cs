using NitroxModel.Logger;
using UnityEngine;

namespace Nitrox.Vr;

public static class VrHud
{
    private static Transform? screenCanvas;
    private static Transform? overlayCanvas;
    private static Transform? hud;

    private static Canvas? staticHudCanvas;

    private static void SetupHandReticle(bool onLaserPointer, Camera uiCamera, Transform rightControllerUI)
    {
        if (onLaserPointer)
        {
            SetupHandReticleLaserPointer(uiCamera, rightControllerUI);
        }
        else
        {
            SetupHandReticleOnHand(uiCamera, rightControllerUI);
        }
    }

    private static void SetupHandReticleOnHand(Camera uiCamera, Transform rightControllerUI)
    {
        // Steal Reticle and attach to the right hand
        GameObject handReticle = HandReticle.main.gameObject.WithParent(rightControllerUI.transform);
        handReticle.GetOrAddComponent<Canvas>().worldCamera = uiCamera;
        handReticle.transform.localEulerAngles = new Vector3(90, 0, 0);
        handReticle.transform.localPosition = new Vector3(0, 0, 0.05f);
        handReticle.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
    }

    private static void SetupHandReticleLaserPointer(Camera uiCamera, Transform rightControllerUI)
    {
        if (VrCameraRig.Instance != null && VrCameraRig.Instance.laserPointerUI == null)
        {
            return;
        }

        if (VrCameraRig.Instance == null || VrCameraRig.Instance.laserPointerUI == null)
        {
            return;
        }

        GameObject handReticle = HandReticle.main.gameObject.WithParent(VrCameraRig.Instance.laserPointerUI.pointerDot.transform);
        handReticle.transform.LookAt(uiCamera.transform.position);
        handReticle.transform.localRotation = Quaternion.Euler(40, 0, 0);
        handReticle.transform.localPosition = new Vector3(0, -5, VrCameraRig.Instance.laserPointerUI.pointerDot.transform.localPosition.z); //new Vector3(0, 0, 0.05f);
        handReticle.transform.localScale = VrCameraRig.Instance.laserPointerUI.pointerDot.transform.localScale * 2; //new Vector3(0.001f, 0.001f, 0.001f);
    }

    private static void OnHandReticleSettingChanged(bool onLaserPointer)
    {
        VrCameraRig? rig = VrCameraRig.Instance;
        if (rig == null || rig.uiCamera == null || rig.rightControllerUI == null)
        {
            return;
        }

        SetupHandReticle(onLaserPointer, rig.uiCamera, rig.rightControllerUI.transform);
    }

    public static Canvas CreateWorldCanvas(this GameObject go)
    {
        Canvas canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        go.layer = LayerID.UI;
        return canvas;
    }

    public static void Setup(Camera uiCamera, Transform rightControllerUI)
    {
        Log.Debug($"Setting up HUD for {uiCamera.name}");

        screenCanvas = uGUI.main.screenCanvas.gameObject.transform;
        overlayCanvas = uGUI.main.overlays.gameObject.transform.parent;
        hud = uGUI.main.hud.transform;

        if (staticHudCanvas == null && VrCameraRig.Instance != null && VrCameraRig.Instance.uiRig != null)
        {
            Transform uiRig = VrCameraRig.Instance.uiRig.transform;
            GameObject go = new GameObject("StaticHUDCanvas").WithParent(uiRig);
            staticHudCanvas = go.CreateWorldCanvas();
            RectTransform? rt = go.GetComponent<RectTransform>();
            go.transform.localScale = screenCanvas.localScale;
            rt.sizeDelta = screenCanvas.GetComponent<RectTransform>().sizeDelta;
            rt.anchoredPosition = screenCanvas.GetComponent<RectTransform>().anchoredPosition;
            go.transform.localPosition = Vector3.forward;
            go.transform.localRotation = Quaternion.identity;
        }

        if (staticHudCanvas != null)
        {
            staticHudCanvas.worldCamera = uiCamera;
        }
        
        // screenCanvas.SetParent(uiCamera.transform, true);
        overlayCanvas.SetParent(uiCamera.transform, true);
        
        SetupHandReticle(Settings.PutHandReticleOnLaserPointer, uiCamera, rightControllerUI);
        Settings.PutHandReticleOnLaserPointerChanged -= OnHandReticleSettingChanged;
        Settings.PutHandReticleOnLaserPointerChanged += OnHandReticleSettingChanged;
        
        WristHud.Setup();
        
        uGUI_CanvasScaler? compo = screenCanvas.GetComponent<uGUI_CanvasScaler>();
        if (compo != null)
        {
            compo.SetDirty();
        }
        screenCanvas.GetComponentsInChildren<uGUI_CanvasScaler>().ForEach(cs => cs.SetDirty());
    }

    public static void OnEnterVehicle()
    {
        Player? player = Player.main;

        if (player == null || staticHudCanvas == null || hud == null)
        {
            return;
        }

        hud.SetParent(staticHudCanvas.transform, false);
    }

    public static void OnExitVehicle()
    {
        if (hud == null)
        {
            return;
        }

        hud.SetParent(screenCanvas, false);
    }
}
