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
        Log.Info("ArmsController_Start");
        Camera mainCamera = SNCameraRoot.main.mainCam;
        if (VrCameraRig.Instance != null)
        {
            VrCameraRig.Instance.SetCameraTrackTarget(mainCamera.transform.parent);
            CoroutineHost.StartCoroutine(VrCameraRig.Instance.SetupGameCameras());
        }

        // Disable IK
        __instance.ik.enabled = false;
        __instance.leftAim.aimer.enabled = false;
        __instance.rightAim.aimer.enabled = false;

        // Attach
        __instance.gameObject.AddComponent<VrHands>().Setup(__instance.ik);
    }
}
