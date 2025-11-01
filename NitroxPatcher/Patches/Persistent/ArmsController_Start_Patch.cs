using System.Reflection;
using Nitrox.Vr;
using UnityEngine;
using UWE;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class ArmsController_Start_Patch : NitroxPatch, IPersistentPatch
{
    public static readonly MethodInfo TARGET_METHOD = Reflect.Method((ArmsController t) => t.Start());

    public static void Postfix(ArmsController __instance)
    {
        __instance.Reconfigure(null);
        
        Camera mainCamera = SNCameraRoot.main.mainCam;
        if (VrCameraRig.Instance != null)
        {
            VrCameraRig.Instance.SetCameraTrackTarget(mainCamera.transform.parent);
            CoroutineHost.StartCoroutine(VrCameraRig.Instance.SetupGameCameras());
            Log.Info("CAMERA SETUP");
        }
    }
}
